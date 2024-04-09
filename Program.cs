using System;
using System.Threading.Tasks;
using Octokit;

class Program
{
    static async Task Main(string[] args)
    {
        var github = new GitHubClient(new ProductHeaderValue("MyAmazingApp"));
        github.Credentials = new Credentials("your-github-token"); // NOTE: replace "your-github-token" with your actual token

        var repositories = await github.Repository.GetAllForOrg("your-organisation"); // NOTE: replace "your-organisation" with your actual organisation name

        foreach (var repo in repositories)
        {
            Console.WriteLine($"Repo: {repo.Name}");

            var stats = await github.Repository.Statistics.GetContributors(repo.Id);

            foreach (var stat in stats)
            {
                Console.WriteLine($"User: {stat.Author.Login}, Contributions: {stat.Total}");
            }
        }
    }
}