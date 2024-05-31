import io
import sys
import json
import asyncio
import httpx
import contextlib

from typing import Any, AsyncGenerator, Callable, Dict, List, Optional, cast

from fastapi_poe.client import get_bot_response, get_final_response

from fastapi_poe.types import (
    ProtocolMessage,
    PartialResponse,
    QueryRequest
)

import silver
from silver import receive_stream_text
from silver import stream_output_text

async def main(argv):

    data = json.loads(argv)

    # 读取字段
    prompt = data.get('prompt')
    messages = transform_messages(prompt)
    model = data.get('model', 'GPT-3.5-Turbo')
    api_key = data.get('api_key')
    temperature = data.get('temperature', 0.5)
    proxy = data.get('proxy', "http://localhost:7890")

    proxies = {
    "http://": proxy,
    "https://": proxy
    }

    async with contextlib.AsyncExitStack() as stack:
        session = await stack.enter_async_context(httpx.AsyncClient(proxies=proxies))
        await invoke(messages, model, api_key, temperature,session)

def transform_messages(prompt):
    messages = []

    if isinstance(prompt, str):
        # prompt 是一个字符串
        messages.append(ProtocolMessage(role='user', content=prompt))
        pass
    elif isinstance(prompt, list):
        # prompt 是一个字典
        for msg in prompt:
            messages.append(ProtocolMessage(role=msg['role'], content=msg['content']))
        pass
    else:
        # prompt 不是字符串也不是字典
        raise Exception("prompt is not a string or json object!")

    return messages

#Claude-instant  Google-PaLM Web-Search
# Create an asynchronous function to encapsulate the async for loop
async def invoke(messages: List[ProtocolMessage],
    bot_name: str,
    api_key: str,
    temperature: Optional[float] = None,
    session: Optional[httpx.AsyncClient] = None
    ):
    try:
        async for partial in get_bot_response(
            messages=messages, bot_name=bot_name, api_key=api_key,
            temperature=temperature, session=session):
            print(partial.text)
            sys.stdout.flush()
    except Exception as e:
        sys.stderr.write(str(e))

if __name__ == "__main__":
    args = receive_stream_text()
    args = args[1:]#去掉第一个诡异字符
    # args2 = '{"api_key":"YyHuc3roVuCDYghHZoKj437xsnlm-5sKIjaHUcdio7Q","model":"GPT-3.5-Turbo","proxy":"http://localhost:7890","prompt":"Write one paragraph why AI is awesome","temperature":1}'
    # args = '{"prompt": "你好", "model": "GPT-3.5-Turbo", "api_key": "YyHuc3roVuCDYghHZoKj437xsnlm-5sKIjaHUcdio7Q", "temperature": 0.5, "proxy": "http://localhost:7890"}'
    asyncio.run(main(args))