using Accord.DataSets;
using Accord.Statistics.Analysis;
using Accord.Statistics;
using BenchmarkDotNet.Attributes;

namespace ConsoleApp3;

public class PCA
{
    [Benchmark]
    public void DoPCA()
    {
        // Iris veri setini yükle
        var iris = new Iris();
        double[][] data = iris.Instances;

        // Veriyi standart ölçeklendirme
        var X = StandardizeData(data);

        // PCA analizi
        var pca = new PrincipalComponentAnalysis(method: PrincipalComponentMethod.Standardize);
        pca.Learn(X);

        // PCA sonuçlarını al
        var eigenvalues = pca.Eigenvalues;
        var eigenvectors = pca.ComponentVectors;

        // Kumulatif açıklanan varyansı hesapla
        var cumulativeExplainedVariance = CumulativeExplainedVariance(eigenvalues);
 
        //PrintPCAResults(eigenvalues, eigenvectors, cumulativeExplainedVariance);
    }

    private void PrintPCAResults(double[] eigenvalues, double[][] eigenvectors, double[] cumulativeExplainedVariance)
    {
        // PCA sonuçlarını ve özvektörleri yazdır
        Console.WriteLine("Eigenvalues:");
        for (int i = 0; i < eigenvalues.Length; i++)
        {
            Console.WriteLine($"Eigenvalue {i + 1}: {eigenvalues[i]}");
        }

        Console.WriteLine("\nEigenvectors:");
        for (var i = 0; i < eigenvectors.Length; i++)
        {
            Console.Write($"EigenVector: {i}: ");
            for (var j = 0; j < eigenvectors[i].Length; j++)
            {
                Console.Write($"{eigenvectors[i][j]} ");
            }
            Console.WriteLine();
        }

        Console.WriteLine("\nCumilative Explained Variance");
        for (var i = 0; i < cumulativeExplainedVariance.Length; i++)
        {
            Console.WriteLine(cumulativeExplainedVariance[i]);
        }
    }

    // Veriyi standart ölçeklendirme
    private double[][] StandardizeData(double[][] data)
    {
        var rows = data.Length;
        var cols = data[0].Length;

        // Her bir sütun için ortalamayı ve standart sapmayı hesapla
        var means = new double[cols];
        var stdDevs = new double[cols];

        for (int col = 0; col < cols; col++)
        {
            var column = data.Select(row => row[col]).ToArray();
            means[col] = column.Average();
            stdDevs[col] = Measures.StandardDeviation(column);
        }

        // Standart ölçeklendirilmiş veriyi oluştur
        var standardizedData = new double[rows][];

        for (int row = 0; row < rows; row++)
        {
            standardizedData[row] = new double[cols];
            for (int col = 0; col < cols; col++)
            {
                standardizedData[row][col] = (data[row][col] - means[col]) / stdDevs[col];
            }
        }

        return standardizedData;
    }

    // Kumulatif açıklanan varyansı hesapla
    private double[] CumulativeExplainedVariance(double[] eigenvalues)
    {
        var totalVariance = eigenvalues.Sum();
        var cumulativeSum = 0.0;
        var cumulativeExplainedVariance = new double[eigenvalues.Length];

        for (int i = 0; i < eigenvalues.Length; i++)
        {
            cumulativeSum += eigenvalues[i];
            cumulativeExplainedVariance[i] = cumulativeSum / totalVariance;
        }

        return cumulativeExplainedVariance;
    }
}
