// Copyright (c) Microsoft. All rights reserved.

using Microsoft.SemanticKernel.Connectors.Memory.Qdrant.Http.ApiSchema;
using System.Text.Json;

namespace ConsoleApp1;

internal class Program
{
    static async Task Main(string[] args)
    {
        await Test2();
    }
    public static async Task Test2()
    {
        var responseContent = "{\"result\":[{\"id\":\"00249787-a197-4b79-b92e-1a223f3772b8\",\"version\":11,\"score\":0.88197064,\"payload\":{\"additional_metadata\":\"{\\\"Index\\\":0,\\\"Next\\\":1,\\\"Previous\\\":0,\\\"UserID\\\":\\\"39083455784@chatroom\\\",\\\"UserName\\\":\\\"\\\"}\",\"description\":\"文化参考《长安三万里》\",\"external_source_name\":\"\",\"id\":\"文化参考《长安三万里》:0\",\"is_reference\":false,\"text\":\"今天来说的是院线新片，由追光动画制作，谢君伟、邹靖执导的动画长片《长安三万里》——这个“长”字真得拖长音来念，因为是真长啊，差10分钟就三个钟头了，观影期间可别多喝汽水。 在现在这个一切都讲压缩、都图快的时候，把院线电影特别是锁定少儿群体的动画电影做得这么长有点儿冒险。我上一次看如此任性的动画还是高畑勋的《辉夜姬物语》，那还比这短半小时呢。 可不可以剪短些呢？要是按商业类型片的紧凑标准去剪，当然也行。可这就是业余观众一时兴起就去质疑人家专业人员的“扶手椅四分位”心态。这么关键的事儿，主创当然都能想到，“是不为也，非不能也”。 现在回想，把这部电影保持在168分钟的节奏是好的，它要实现一种与之匹配的气质：在这部《长安三万里》当中，完成了一次系统的中国美学探索，拍出了一部关于盛唐的史诗。\"},\"vector\":null},{\"id\":\"e1041350-a2ca-41c8-b011-c5121fddb3f1\",\"version\":17,\"score\":0.8770933,\"payload\":{\"additional_metadata\":\"{\\\"Index\\\":3,\\\"Next\\\":4,\\\"Previous\\\":2,\\\"UserID\\\":\\\"39083455784@chatroom\\\",\\\"UserName\\\":\\\"\\\"}\",\"description\":\"文化参考《长安三万里》\",\"external_source_name\":\"\",\"id\":\"文化参考《长安三万里》:3\",\"is_reference\":false,\"text\":\"《长安三万里》的策略是：从诗歌的视角去讲盛唐。这个选择很有主观上的真实感，我们对天宝时代的印象就是由唐诗和诗人组成的。在这部电影里，正史里的关键人物，像玄宗父子、安禄山、李林甫、杨玉环，都没有出场，只在台词里 一笔带过。出场的角色差不多都是诗人，像郭子仪、哥舒翰这些军事人物，也是因为和诗人有交往，可以说是“惟有文化留其名”。片中李白人生的中心场地黄鹤楼，以及温柔富贵的扬州城和雄壮的长安城，构成了一片“文化的江山”。 故事主视角是哪个诗人呢？不是李白，是和他有很深交往的边塞诗人高适，这也是个好选择。高适、李白和杜甫三位大诗人曾经一起共游梁园，他们诗酒订交的场面也是中国文 学史上的一大高光时刻。高适在唐代诗人里介入时代最深，也可以说是世俗成就最大，他被唐肃宗李亨重用，做到淮南节度使的高位，参与平定了安史之乱期间的“永王之乱”。\"},\"vector\":null}],\"status\":\"ok\",\"time\":0.0015082}";

        //var data = JsonSerializer.Deserialize<SearchVectorsResponse>(responseContent);
    }
}
