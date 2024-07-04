using Azure;
using Azure.AI.OpenAI;
using Milvus.Client;
using OpenAI.Embeddings;
using System.ClientModel;
using System.Numerics.Tensors;

namespace UnitTests.Milvus;

public class MilvusTest(ITestOutputHelper output) : BaseTest(output)
{
    const string collectionName = "test";

    private readonly MilvusDbClient milvusDbClient = new("127.0.0.1", 19530);

    [Fact]
    public async Task RunCreateCollectionAsync()
    {
        bool result = await milvusDbClient.CreateCollectionAsync(collectionName);

        Assert.True(result);
    }

    [Fact]
    public async Task RunUpsertEntitiesAsync()
    {
        foreach (SearchAndContent sc in this.SearchAndContent)
        {
            ReadOnlyMemory<float> embeddings = await this.GetEmbeddingAsync(sc.Search);

            IReadOnlyList<string>? ids = await milvusDbClient.UpsertEntitiesAsync(collectionName, Guid.NewGuid().ToString(), sc.Search, sc.Content, embeddings);

            Assert.NotNull(ids);
            Assert.True(ids.Any());
        }
    }


    [Fact]
    public async Task RunSearchAsync()
    {
        string search = "前部和后部均有口袋设计";

        ReadOnlyMemory<float> embeddings = await this.GetEmbeddingAsync(search);

        SearchResults searchResult = await milvusDbClient.SearchEntitiesAsync(collectionName, embeddings, 2);

        Assert.NotNull(searchResult);

        foreach (var fieldData in searchResult.FieldsData)
        {
            if (fieldData.FieldName == "content")
            {
                foreach (var data in (fieldData as FieldData<string>)!.Data)
                {
                    this.WriteLine(data);
                    this.WriteLine("=======================================================================");
                }
            }
        }
    }

    [Fact]
    public async Task CalcCosineSimilarity()
    {
        //string content1 = SearchAndContent[0].Search;
        //string content2 = SearchAndContent[1].Search;

        string content1 = SearchAndContent[1].Search;
        string content2 = SearchAndContent[2].Search;

        //string content1 = SearchAndContent[2].Search;
        //string content2 = SearchAndContent[3].Search;

        //string content1 = SearchAndContent[0].Search;
        //string content2 = SearchAndContent[3].Search;

        //string content1 = SearchAndContent[0].Search;
        //string content2 = SearchAndContent[2].Search;

        //string content1 = SearchAndContent[1].Search;
        //string content2 = SearchAndContent[3].Search;


        ReadOnlyMemory<float> embeddings1 = await this.GetEmbeddingAsync(content1);

        ReadOnlyMemory<float> embeddings2 = await this.GetEmbeddingAsync(content2);

        double similarity = TensorPrimitives.CosineSimilarity(embeddings1.Span, embeddings2.Span);

        Console.WriteLine(similarity);
    }

    private async Task<ReadOnlyMemory<float>> GetEmbeddingAsync(string search)
    {
        AzureOpenAIClient azureOpenAIClient = new AzureOpenAIClient(new Uri(TestConfiguration.AzureOpenAIEmbeddings.Endpoint), new AzureKeyCredential(TestConfiguration.AzureOpenAIEmbeddings.ApiKey));

        EmbeddingClient embeddingClient = azureOpenAIClient.GetEmbeddingClient(TestConfiguration.AzureOpenAIEmbeddings.DeploymentName);

        ClientResult<Embedding> embeddingClientResult = await embeddingClient.GenerateEmbeddingAsync(search);

        return embeddingClientResult.Value.Vector;
    }


    readonly List<SearchAndContent> SearchAndContent =
    [
        new SearchAndContent{
            Search = "Nike最新牛仔裤 ### 材质\n这款短裤主要由牛仔布材质制成。牛仔布以其耐用性和耐磨损性闻名，适合日常穿着和户外活动。\n\n### 设计细节\n- 前部和后部均有口袋设计，提供实用的储物空间。\n- 配有腰带环，搭配皮带使用，更加稳固和时尚。\n- 拉链和纽扣扣合，确保穿脱方便。\n- 内衬有条纹装饰，增添了设计感和细节美感。\n\n### 版型\n这款短裤采用直筒版型，适合各种体型的人穿着。裤腿稍微宽松，提供了良好的活动空间，适合夏季穿着。\n\n### 特点\n- 牛仔布材质确保耐用性和舒适性。\n- 多口袋设计提高了实用性，适合户外活动和日常穿着。\n- 内衬条纹设计增加了时尚感。\n- 腰带环设计方便搭配不同风格的皮带，增强整体造型感。",
            Content = "### Nike个性牛仔裤\n\n### 模特口播\n\n哈喽宝子们，今天来跟大家分享一下这款全新的Nike牛仔裤【近景，打开包装】\n\n这款牛仔裤的设计真的太棒了，有前后口袋设计，不仅时尚，还非常实用，适合各种场景穿搭哦【①主播侧拿牛仔裤，镜头拍主播+裤子近景；②裤子放在展示台上特写】\n\n它采用的是高质量牛仔布材质，耐磨耐用，穿着非常舒适，夏季穿也不会觉得闷热【展示裤面，口播的同时手在裤面比划】\n\n裤子的直筒版型，裤腿稍微宽松，给你足够的活动空间，无论是日常还是户外活动都非常合适【①主播垂直展示牛仔裤整体，镜头聚焦裤腿位置；②主播上身展示，特写裤腿】\n\n不仅如此，腰带环设计让你可以搭配各种风格的皮带，增强整体造型感，真的超赞【①主播手持，比划腰带环；②上身穿搭效果特写】\n\n来看这款牛仔裤的上身效果【全景，摆pose】\n\n是不是好看又实用，快点击下方购物车入手吧！【主播近景】\n\n### 后期花字\n\nNike开箱来了\n\nNike个性牛仔裤\n\n耐磨牛仔布，舒适耐用\n\n多口袋设计，时尚实用\n\n直筒版型，适合各种体型"
        },
        new SearchAndContent{
            Search = "Nike最新板鞋 ### 材质\n这款鞋采用了优质皮革作为主要材质，提供良好的耐用性和支撑性。鞋面的皮革不仅增加了鞋子的耐用性，还带来了经典的外观和质感。\n\n### 设计细节\n鞋面采用了对比色的拼接设计，白色和蓝色的搭配带来了视觉上的冲击力。鞋头和鞋侧面的透气孔设计，增强了鞋子的透气性。鞋帮处的缝线细节，以及鞋跟处的刺绣图案，增添了独特的设计感。\n\n### 版型\n这款鞋的版型为经典的低帮设计，适合日常穿着，提供了良好的灵活性和舒适性。低帮设计也让脚踝能够自由活动，适合各种场合穿着。\n\n### 特点\n- **鞋面**: 采用优质皮革，增加耐用性和支撑性。透气孔设计提升舒适度。\n- **鞋底**: 鞋底采用耐磨橡胶材料，提供良好的抓地力和耐用性。经典的波浪纹鞋底设计，增加了摩擦力，防滑性能佳。\n\n通过以上分析，可以看出这款鞋既有经典的设计，又兼具实用性和舒适性，适合日常穿着和多种活动场合。",
            Content = "### Nike经典板鞋\n\n### 模特口播\n\n哈喽宝子们，今天给大家带来一双全新的Nike经典板鞋！【近景，打开鞋盒】\n\n这白蓝配色的拼接设计实在是太炫酷了，简直就是视觉的冲击波！【①主播侧拿鞋子，镜头拍主播+鞋近景；②鞋子放在鞋盒上特写】\n\n来看看这个鞋面，采用的是优质皮革材质，手感非常细腻，还不失耐用性和支撑性【展示鞋面，口播的同时手在鞋面比划】\n\n鞋底采用耐磨橡胶材料，不仅抓地力强、耐用性高，还搭载了经典的波浪纹设计，防滑效果非常不错【①主播垂直展示鞋子正面和侧面，然后镜头聚焦鞋底位置；②主播上脚，足跟踩压鞋垫，特写】\n\n鞋帮处的缝线细节以及鞋跟处的刺绣图案，真的是个性满满，低帮设计让脚踝自由活动，舒适又灵活【①主播手持，比划鞋底；②上脚足部特写】\n\n来看看上身效果【全景,摆pose】\n\n是不是好看又实用，快点击下方购物车入手吧！【主播近景】\n\n### 后期花字\n\nNIKE开箱来了\n\nNike经典板鞋\n\n优质皮革，耐用支撑\n\n白蓝拼接，视觉冲击\n\n耐磨橡胶，抓地防滑"
        },
        new SearchAndContent{
            Search = "2024新款运动鞋 ### 材质\n- 鞋面：网眼织物和合成材料，提供透气性和支撑。\n- 鞋底：橡胶材质，具有耐磨性和抓地力。\n\n### 设计细节\n- 鞋面采用无缝设计，减少摩擦并提升舒适度。\n- 鞋侧的大号白色 Nike Swoosh 标志，凸显品牌识别度。\n- 鞋舌和鞋跟处有额外的填充物，增加舒适性。\n\n### 版型\n- 低帮设计，提供灵活的脚踝运动。\n- 标准鞋楦，适合大多数脚型。\n\n### 特点\n#### 鞋面\n- 网眼材质，增强透气性。\n- 合成覆盖层，提供额外的支撑和耐用性。\n\n#### 鞋底\n- 橡胶外底，具有良好的耐磨性。\n- 中底采用泡棉材料，提供轻质缓震效果。\n- 鞋底纹路设计，增强多种地面条件下的抓地力。",
            Content = "### 2024新款运动鞋\n\n### 模特口播\n\n哈喽宝子们！来看看我热情推荐带来的2024新款运动鞋，真的是绝佳的必入单品哦！【近景，打开鞋盒】\n\n这双鞋的配色真的让人眼前一亮，有种纯净的科技感，特别适合我们日常穿搭和运动场景。【①主播侧拿鞋子，镜头拍主播+鞋近景；②鞋子放在鞋盒上特写】\n\n鞋面采用了舒适网眼织物和合成材料，透气性一级棒，还能提供良好的支撑，穿上它走再久都不会闷！【展示鞋面，口播的同时手在鞋面比划】\n\n鞋底橡胶材质设计，耐磨性能非常棒，再加上鞋底纹路设计，抓地力超强，跑步、健身都妥妥的！【①主播垂直展示鞋子正面和侧面，然后镜头聚焦鞋底位置；②主播上脚，足跟踩压鞋垫，特写】\n\n鞋舌和鞋跟处有额外的填充物，穿上它就像脚在享受SPA一样舒适，低帮设计也让脚踝更灵活自如。【①主播手持，比划鞋底；②上脚足部特写】\n\n来看看上身效果，是不是超酷的！【全景,摆pose】\n\n是不是好看又实用，快点击下方购物车入手吧！【主播近景】\n\n### 后期花字\n\nNike开箱来了\n2024新款运动鞋\n\n网眼织物鞋面，透气支撑  \n橡胶外底，耐磨抓地  \n轻质泡棉中底，缓震效果好  \n低帮设计，灵活自如"
        },
        new SearchAndContent{
            Search = "夏日清爽T恤 ### 材质\n这款T恤采用高性能的聚酯纤维材质，具有良好的透气性和速干功能，确保在运动过程中保持干爽舒适。\n\n### 设计细节\n- **肩部拼接**：肩部采用拼接设计，增加了衣服的结构性和美观度。\n- **胸前印花**：胸前有一个大号的Nike Swoosh标志和“AIR”字样，采用抽象画风设计，充满动感和个性。\n- **简约设计**：整体设计简洁大方，适合多种场合穿着。\n\n### 版型\n这款T恤采用标准版型设计，适合大多数人的身材。它的剪裁略微宽松，提供良好的活动自由度。\n\n### 特点\n- **透气性**：采用透气材质，确保在运动中保持凉爽。\n- **速干**：高性能聚酯纤维能够快速排汗，保持身体干爽。\n- **时尚设计**：胸前的印花设计简约而不失个性，适合日常穿着及轻运动。",
            Content = "### 夏日清爽T恤\n\n### 模特口播\n\n哈喽宝子们，[今天]要给大家介绍的这款夏日清爽T恤，[实在]是这个夏天的[必备]单品哦！【近景，打开包装】\n\n这个清新的配色，一眼就让人感觉到满满的夏日气息！【①主播侧拿T恤，镜头拍主播+T恤近景；②T恤放在桌上特写】\n\n这款T恤采用了高性能的聚酯纤维材质，拥有超强的透气性和速干功能，即使在[炎热]的夏天也能保持干爽舒适【展示T恤，口播的同时手在T恤上比划】\n\n肩部拼接设计不仅增加了结构性，还提升了整体的美观度【①主播垂直展示T恤肩部，镜头聚焦肩部位置；②主播上身展示，侧身特写】\n\n胸前这大号的Nike Swoosh标志和\"AIR\"字样，采用了抽象画风设计，充满了动感和个性，是不是特别潮！【①主播手持，比划胸前印花；②上身特写】\n\n再来看它的版型，标准版型设计适合大多数人，略微宽松的剪裁提供了良好的活动自由度，穿着既舒适又时尚【①主播手持，展示整体版型；②上身展示，整体效果】\n\n来看看上身效果【全景,摆pose】\n\n是不是好看又实用，快点击下方购物车入手吧！【主播近景】\n\n### 后期花字\n\nNike开箱来了\n\n夏日清爽T恤\n\n高性能聚酯纤维，透气速干\n\n肩部拼接，结构美观\n\n大号Swoosh印花，动感个性"
        }
    ];

    /*
     * 用户问题+识图结果 文案内容
     Nike最新牛仔裤
     {    "text": "### 材质\n这款短裤主要由牛仔布材质制成。牛仔布以其耐用性和耐磨损性闻名，适合日常穿着和户外活动。\n\n### 设计细节\n- 前部和后部均有口袋设计，提供实用的储物空间。\n- 配有腰带环，搭配皮带使用，更加稳固和时尚。\n- 拉链和纽扣扣合，确保穿脱方便。\n- 内衬有条纹装饰，增添了设计感和细节美感。\n\n### 版型\n这款短裤采用直筒版型，适合各种体型的人穿着。裤腿稍微宽松，提供了良好的活动空间，适合夏季穿着。\n\n### 特点\n- 牛仔布材质确保耐用性和舒适性。\n- 多口袋设计提高了实用性，适合户外活动和日常穿着。\n- 内衬条纹设计增加了时尚感。\n- 腰带环设计方便搭配不同风格的皮带，增强整体造型感。",    "images": []  }
     {    "text": "### Nike个性牛仔裤\n\n### 模特口播\n\n哈喽宝子们，今天来跟大家分享一下这款全新的Nike牛仔裤【近景，打开包装】\n\n这款牛仔裤的设计真的太棒了，有前后口袋设计，不仅时尚，还非常实用，适合各种场景穿搭哦【①主播侧拿牛仔裤，镜头拍主播+裤子近景；②裤子放在展示台上特写】\n\n它采用的是高质量牛仔布材质，耐磨耐用，穿着非常舒适，夏季穿也不会觉得闷热【展示裤面，口播的同时手在裤面比划】\n\n裤子的直筒版型，裤腿稍微宽松，给你足够的活动空间，无论是日常还是户外活动都非常合适【①主播垂直展示牛仔裤整体，镜头聚焦裤腿位置；②主播上身展示，特写裤腿】\n\n不仅如此，腰带环设计让你可以搭配各种风格的皮带，增强整体造型感，真的超赞【①主播手持，比划腰带环；②上身穿搭效果特写】\n\n来看这款牛仔裤的上身效果【全景，摆pose】\n\n是不是好看又实用，快点击下方购物车入手吧！【主播近景】\n\n### 后期花字\n\nNike开箱来了\n\nNike个性牛仔裤\n\n耐磨牛仔布，舒适耐用\n\n多口袋设计，时尚实用\n\n直筒版型，适合各种体型",    "images": []  }

    Nike最新板鞋
    {    "text": "### 材质\n这款鞋采用了优质皮革作为主要材质，提供良好的耐用性和支撑性。鞋面的皮革不仅增加了鞋子的耐用性，还带来了经典的外观和质感。\n\n### 设计细节\n鞋面采用了对比色的拼接设计，白色和蓝色的搭配带来了视觉上的冲击力。鞋头和鞋侧面的透气孔设计，增强了鞋子的透气性。鞋帮处的缝线细节，以及鞋跟处的刺绣图案，增添了独特的设计感。\n\n### 版型\n这款鞋的版型为经典的低帮设计，适合日常穿着，提供了良好的灵活性和舒适性。低帮设计也让脚踝能够自由活动，适合各种场合穿着。\n\n### 特点\n- **鞋面**: 采用优质皮革，增加耐用性和支撑性。透气孔设计提升舒适度。\n- **鞋底**: 鞋底采用耐磨橡胶材料，提供良好的抓地力和耐用性。经典的波浪纹鞋底设计，增加了摩擦力，防滑性能佳。\n\n通过以上分析，可以看出这款鞋既有经典的设计，又兼具实用性和舒适性，适合日常穿着和多种活动场合。",    "images": []  }
    {    "text": "### Nike经典板鞋\n\n### 模特口播\n\n哈喽宝子们，今天给大家带来一双全新的Nike经典板鞋！【近景，打开鞋盒】\n\n这白蓝配色的拼接设计实在是太炫酷了，简直就是视觉的冲击波！【①主播侧拿鞋子，镜头拍主播+鞋近景；②鞋子放在鞋盒上特写】\n\n来看看这个鞋面，采用的是优质皮革材质，手感非常细腻，还不失耐用性和支撑性【展示鞋面，口播的同时手在鞋面比划】\n\n鞋底采用耐磨橡胶材料，不仅抓地力强、耐用性高，还搭载了经典的波浪纹设计，防滑效果非常不错【①主播垂直展示鞋子正面和侧面，然后镜头聚焦鞋底位置；②主播上脚，足跟踩压鞋垫，特写】\n\n鞋帮处的缝线细节以及鞋跟处的刺绣图案，真的是个性满满，低帮设计让脚踝自由活动，舒适又灵活【①主播手持，比划鞋底；②上脚足部特写】\n\n来看看上身效果【全景,摆pose】\n\n是不是好看又实用，快点击下方购物车入手吧！【主播近景】\n\n### 后期花字\n\nNIKE开箱来了\n\nNike经典板鞋\n\n优质皮革，耐用支撑\n\n白蓝拼接，视觉冲击\n\n耐磨橡胶，抓地防滑",    "images": []  }

     2024新款运动鞋
    {    "text": "### 材质\n- 鞋面：网眼织物和合成材料，提供透气性和支撑。\n- 鞋底：橡胶材质，具有耐磨性和抓地力。\n\n### 设计细节\n- 鞋面采用无缝设计，减少摩擦并提升舒适度。\n- 鞋侧的大号白色 Nike Swoosh 标志，凸显品牌识别度。\n- 鞋舌和鞋跟处有额外的填充物，增加舒适性。\n\n### 版型\n- 低帮设计，提供灵活的脚踝运动。\n- 标准鞋楦，适合大多数脚型。\n\n### 特点\n#### 鞋面\n- 网眼材质，增强透气性。\n- 合成覆盖层，提供额外的支撑和耐用性。\n\n#### 鞋底\n- 橡胶外底，具有良好的耐磨性。\n- 中底采用泡棉材料，提供轻质缓震效果。\n- 鞋底纹路设计，增强多种地面条件下的抓地力。",    "images": []  }
    {    "text": "### 2024新款运动鞋\n\n### 模特口播\n\n哈喽宝子们！来看看我热情推荐带来的2024新款运动鞋，真的是绝佳的必入单品哦！【近景，打开鞋盒】\n\n这双鞋的配色真的让人眼前一亮，有种纯净的科技感，特别适合我们日常穿搭和运动场景。【①主播侧拿鞋子，镜头拍主播+鞋近景；②鞋子放在鞋盒上特写】\n\n鞋面采用了舒适网眼织物和合成材料，透气性一级棒，还能提供良好的支撑，穿上它走再久都不会闷！【展示鞋面，口播的同时手在鞋面比划】\n\n鞋底橡胶材质设计，耐磨性能非常棒，再加上鞋底纹路设计，抓地力超强，跑步、健身都妥妥的！【①主播垂直展示鞋子正面和侧面，然后镜头聚焦鞋底位置；②主播上脚，足跟踩压鞋垫，特写】\n\n鞋舌和鞋跟处有额外的填充物，穿上它就像脚在享受SPA一样舒适，低帮设计也让脚踝更灵活自如。【①主播手持，比划鞋底；②上脚足部特写】\n\n来看看上身效果，是不是超酷的！【全景,摆pose】\n\n是不是好看又实用，快点击下方购物车入手吧！【主播近景】\n\n### 后期花字\n\nNike开箱来了\n2024新款运动鞋\n\n网眼织物鞋面，透气支撑  \n橡胶外底，耐磨抓地  \n轻质泡棉中底，缓震效果好  \n低帮设计，灵活自如",    "images": []  }


    夏日清爽T恤
    {    "text": "### 材质\n这款T恤采用高性能的聚酯纤维材质，具有良好的透气性和速干功能，确保在运动过程中保持干爽舒适。\n\n### 设计细节\n- **肩部拼接**：肩部采用拼接设计，增加了衣服的结构性和美观度。\n- **胸前印花**：胸前有一个大号的Nike Swoosh标志和“AIR”字样，采用抽象画风设计，充满动感和个性。\n- **简约设计**：整体设计简洁大方，适合多种场合穿着。\n\n### 版型\n这款T恤采用标准版型设计，适合大多数人的身材。它的剪裁略微宽松，提供良好的活动自由度。\n\n### 特点\n- **透气性**：采用透气材质，确保在运动中保持凉爽。\n- **速干**：高性能聚酯纤维能够快速排汗，保持身体干爽。\n- **时尚设计**：胸前的印花设计简约而不失个性，适合日常穿着及轻运动。",    "images": []  }
    {    "text": "### 夏日清爽T恤\n\n### 模特口播\n\n哈喽宝子们，[今天]要给大家介绍的这款夏日清爽T恤，[实在]是这个夏天的[必备]单品哦！【近景，打开包装】\n\n这个清新的配色，一眼就让人感觉到满满的夏日气息！【①主播侧拿T恤，镜头拍主播+T恤近景；②T恤放在桌上特写】\n\n这款T恤采用了高性能的聚酯纤维材质，拥有超强的透气性和速干功能，即使在[炎热]的夏天也能保持干爽舒适【展示T恤，口播的同时手在T恤上比划】\n\n肩部拼接设计不仅增加了结构性，还提升了整体的美观度【①主播垂直展示T恤肩部，镜头聚焦肩部位置；②主播上身展示，侧身特写】\n\n胸前这大号的Nike Swoosh标志和\"AIR\"字样，采用了抽象画风设计，充满了动感和个性，是不是特别潮！【①主播手持，比划胸前印花；②上身特写】\n\n再来看它的版型，标准版型设计适合大多数人，略微宽松的剪裁提供了良好的活动自由度，穿着既舒适又时尚【①主播手持，展示整体版型；②上身展示，整体效果】\n\n来看看上身效果【全景,摆pose】\n\n是不是好看又实用，快点击下方购物车入手吧！【主播近景】\n\n### 后期花字\n\nNike开箱来了\n\n夏日清爽T恤\n\n高性能聚酯纤维，透气速干\n\n肩部拼接，结构美观\n\n大号Swoosh印花，动感个性",    "images": []  }
     */
}

public class SearchAndContent
{
    public string Search { get; set; }
    public string Content { get; set; }
}

/*
 
 
 */