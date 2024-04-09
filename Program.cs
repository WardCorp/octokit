using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Octokit;

class Program
{
    static async Task Main(string[] args)
    {
        var github = new GitHubClient(new ProductHeaderValue("MyAmazingApp"));
        var token = Environment.GetEnvironmentVariable("GH_TOKEN");
        github.Credentials = new Credentials(token);

        var repositories = await github.Repository.GetAllForOrg("your-organisation"); // NOTE: replace "your-organisation" with your actual organisation name

        var contributors = new Dictionary<string, int>();

        foreach (var repo in repositories)
        {
            var stats = await github.Repository.Statistics.GetContributors(repo.Id);

            foreach (var stat in stats)
            {
                if (contributors.ContainsKey(stat.Author.Login))
                {
                    contributors[stat.Author.Login] += stat.Total;
                }
                else
                {
                    contributors[stat.Author.Login] = stat.Total;
                }
            }
        }

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