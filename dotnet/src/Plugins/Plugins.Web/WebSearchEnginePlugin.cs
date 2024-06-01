// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.SemanticKernel.Plugins.Web;

/// <summary>
/// Web search engine plugin (e.g. Bing).
/// </summary>
public class WebSearchEnginePlugin
{
    /// <summary>
    /// The count parameter name.
    /// </summary>
    public const string CountParam = "count";

    /// <summary>
    /// The offset parameter name.
    /// </summary>
    public const string OffsetParam = "offset";

    private readonly IWebSearchEngineConnector _connector;

    /// <summary>
    /// Initializes a new instance of the <see cref="WebSearchEnginePlugin"/> class.
    /// </summary>
    /// <param name="connector">The web search engine connector.</param>
    public WebSearchEnginePlugin(IWebSearchEngineConnector connector)
    {
        this._connector = connector;
    }

    /// <summary>
    /// Performs a web search using the provided query, count, and offset.
    /// </summary>
    /// <param name="query">The text to search for.</param>
    /// <param name="count">The number of results to return. Default is 1.</param>
    /// <param name="offset">The number of results to skip. Default is 0.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The value of the TResult parameter contains the search results as a string.</returns>
    private async Task<IEnumerable<WebPage>> Search10WebPagesAsync(
        string query, int count = 10, int offset = 0, CancellationToken cancellationToken = default)
    {
        var results = await this._connector.SearchAsync<WebPage>(query, count, offset, cancellationToken).ConfigureAwait(false);
        if (!results.Any())
        {
            return new List<WebPage>();
            //throw new InvalidOperationException("Failed to get a response from the web search engine.");
        }

        return results;
    }
    /// <summary>
    /// Performs a web search using the provided query, count, and offset.
    /// 移除 URL 重复的项
    /// </summary>
    /// <param name="query">The text to search for.</param>
    /// <param name="count">The number of results to return. Default is 1.</param>
    /// <param name="offset">The number of results to skip. Default is 0.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The value of the TResult parameter contains the search results as a string.</returns>
    public async Task<List<WebPage>> SearchWebPagesAsync(
    string query, int count = 10, int offset = 0, CancellationToken cancellationToken = default)
    {
        var allResults = new List<WebPage>();
        while (count > 0)
        {
            var batchSize = Math.Min(count, 10);
            var results = await this.Search10WebPagesAsync(query, batchSize, offset, cancellationToken).ConfigureAwait(true);
            allResults.AddRange(results);
            count -= batchSize;
            offset += batchSize;
        }
        allResults = allResults
            .GroupBy(webPage => webPage.Url)
            .Select(group => group.First())// 移除 URL 重复的项
            .ToList();
        return allResults;
    }

    /// <summary>
    /// Performs a web search using the provided query, count, and offset.
    /// </summary>
    /// <param name="query">The text to search for.</param>
    /// <param name="count">The number of results to return. Default is 1.</param>
    /// <param name="offset">The number of results to skip. Default is 0.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The value of the TResult parameter contains the search results as a string.</returns>
    [KernelFunction, Description("Perform a web search.")]
    public virtual async Task<string> SearchAsync(
        [Description("Search query")] string query,
        [Description("Number of results")] int count = 10,
        [Description("Number of results to skip")] int offset = 0,
        CancellationToken cancellationToken = default)
    {
        var results = await this._connector.SearchAsync<string>(query, count, offset, cancellationToken).ConfigureAwait(false);
        if (!results.Any())
        {
            throw new InvalidOperationException("Failed to get a response from the web search engine.");
        }
        return count == 1
        ? results.FirstOrDefault() ?? string.Empty
        : string.Join("\n", results);
        //: JsonSerializer.Serialize(results);
    }

    /// <summary>
    /// Performs a web search using the provided query, count, and offset.
    /// </summary>
    /// <param name="query">The text to search for.</param>
    /// <param name="count">The number of results to return. Default is 1.</param>
    /// <param name="offset">The number of results to skip. Default is 0.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The return value contains the search results as an IEnumerable WebPage object serialized as a string</returns>
    [KernelFunction, Description("Perform a web search and return complete results.")]
    public async Task<string> GetSearchResultsAsync(
        [Description("Text to search for")] string query,
        [Description("Number of results")] int count = 1,
        [Description("Number of results to skip")] int offset = 0,
        CancellationToken cancellationToken = default)
    {
        IEnumerable<WebPage>? results = null;
        try
        {
            results = await this._connector.SearchAsync<WebPage>(query, count, offset, cancellationToken).ConfigureAwait(false);
            if (!results.Any())
            {
                throw new InvalidOperationException("Failed to get a response from the web search engine.");
            }
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }

        return JsonSerializer.Serialize(results);
    }
}
