using JiebaNet.Segmenter;

class BM25Calculator
{
    private double k1 = 1.5;
    private double b = 0.75;
    private int N; // Total number of documents  
    private double avgdl; // Average document length  

    public BM25Calculator(int totalDocuments, double averageDocumentLength)
    {
        N = totalDocuments;
        avgdl = averageDocumentLength;
    }

    public double ComputeBM25(string query, string document)
    {
        var segmenter = new JiebaSegmenter();
        var queryTerms = segmenter.Cut(query).ToList();
        var docTerms = segmenter.Cut(document).ToList();
        var docLength = docTerms.Count;

        var termFrequency = docTerms.GroupBy(t => t)
                                    .ToDictionary(g => g.Key, g => g.Count());

        double score = 0.0;

        foreach (var term in queryTerms)
        {
            if (!termFrequency.ContainsKey(term))
                continue;

            int tf = termFrequency[term];
            int df = termFrequency.ContainsKey(term) ? 1 : 0; // Since we have only one document  

            double idf = Math.Log((N - df + 0.5) / (df + 0.5) + 1);
            double termScore = idf * (tf * (k1 + 1)) / (tf + k1 * (1 - b + b * (docLength / avgdl)));

            score += termScore;
        }

        return score;
    }
}

class Program
{
    static void Main()
    {
        string query = "示例查询";
        string document = "这是一个包含查询词的示例文档";

        // Assuming we have only one document, so N = 1 and avgdl = document length  
        BM25Calculator bm25 = new BM25Calculator(totalDocuments: 1, averageDocumentLength: document.Length);

        double score = bm25.ComputeBM25(query, document);
        Console.WriteLine($"BM25 Score: {score}");
    }
}