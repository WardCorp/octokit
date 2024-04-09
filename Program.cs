using Octokit;

class Program
{
    static async Task Main(string[] args)
    {
        var github = new GitHubClient(new ProductHeaderValue("MyAmazingApp"));
        var token = Environment.GetEnvironmentVariable("GH_TOKEN");
        github.Credentials = new Credentials(token);

        var repositories = await github.Repository.GetAllForOrg("WardCorp"); // NOTE: replace "your-organisation" with your actual organisation name

        var contributors = new Dictionary<string, int>();

        contributors = repositories
            .SelectMany(repo => github.Repository.Statistics.GetContributors(repo.Id).Result)
            .GroupBy(stat => stat.Author.Login)
            .ToDictionary(group => group.Key, group => group.Sum(stat => stat.Total));

        using (var writer = new StreamWriter("output.csv"))
        {
            writer.WriteLine("Author,Contributions");

            foreach (var contributor in contributors)
            {
                writer.WriteLine($"{contributor.Key},{contributor.Value}");
            }
        }

        Console.WriteLine("Data written to output.csv");
    }
}