﻿using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

using SendGrid.WebApi.Models;

namespace SendGrid.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            BlocksTest().Wait();

            Console.WriteLine("=================");

            BouncesTest().Wait();

            Console.WriteLine("=================");

            InvalidEmailsTest().Wait();

            Console.WriteLine("=================");

            ParseWebhookTest().Wait();

            ProfileTest().Wait();

            SpamReportsTest().Wait();

            TimezoneTest().Wait();

            Console.WriteLine("=================");

            //UnsubscribesTest().Wait();

            EnforcedTlsTest().Wait();

            Console.ReadKey();
        }

        private static async Task BlocksTest()
        {
            var account = SendGridAccount.Parse(ConfigurationManager.ConnectionStrings["SendGrid"].ConnectionString);

            var client = account.CreateWebApiClient();

            var result = await client.Blocks.GetAsync(new GetBlocksParameter
            {
                Limit = 10
            });

            foreach (var item in result)
            {
                Console.WriteLine(item.Reason);
            }
        }

        private static async Task BouncesTest()
        {
            var account = SendGridAccount.Parse(ConfigurationManager.ConnectionStrings["SendGrid"].ConnectionString);

            var client = account.CreateWebApiClient();

            var result = await client.Bounces.GetAsync(new GetBouncesParameter
            {
                Days = 1,
            });

            foreach (var item in result.Take(5))
            {
                Console.WriteLine(item.Reason);
            }
        }

        private static async Task CredentialsTest()
        {
            var account = SendGridAccount.Parse(ConfigurationManager.ConnectionStrings["SendGrid"].ConnectionString);

            var client = account.CreateWebApiClient();

            var result = await client.Credentials.GetAsync(new GetCredentialsParameter());

            foreach (var item in result)
            {
                Console.WriteLine(item.Name);
            }
        }

        private static async Task InvalidEmailsTest()
        {
            var account = SendGridAccount.Parse(ConfigurationManager.ConnectionStrings["SendGrid"].ConnectionString);

            var client = account.CreateWebApiClient();

            var result = await client.InvalidEmails.GetAsync(new GetInvalidEmailsParameter
            {
                Days = 10
            });

            foreach (var item in result.Take(5))
            {
                Console.WriteLine(item.Reason);
            }
        }

        private static async Task ParseWebhookTest()
        {
            var account = SendGridAccount.Parse(ConfigurationManager.ConnectionStrings["SendGrid"].ConnectionString);

            var client = account.CreateWebApiClient();

            var result = await client.ParseWebhook.GetAsync();

            foreach (var item in result)
            {
                Console.WriteLine(item.Hostname);
            }
        }

        private static async Task ProfileTest()
        {
            var account = SendGridAccount.Parse(ConfigurationManager.ConnectionStrings["SendGrid"].ConnectionString);

            var client = account.CreateWebApiClient();

            var result = await client.Profile.GetAsync();
        }

        private static async Task SpamReportsTest()
        {
            var account = SendGridAccount.Parse(ConfigurationManager.ConnectionStrings["SendGrid"].ConnectionString);

            var client = account.CreateWebApiClient();

            var result = await client.SpamReports.GetAsync(new GetSpamReportsParameter
            {
                Days = 10
            });

            foreach (var item in result.Take(5))
            {
                Console.WriteLine(item.Email);
            }
        }

        private static async Task TimezoneTest()
        {
            var account = SendGridAccount.Parse(ConfigurationManager.ConnectionStrings["SendGrid"].ConnectionString);

            var client = account.CreateWebApiClient();

            var result = await client.Timezone.GetAsync();

            Console.WriteLine(result.Display);

            var list = await client.Timezone.ListAsync(new ListTimezoneParameter());

            foreach (var item in list)
            {
                Console.WriteLine(item.Timezone);
            }
        }

        private static async Task UnsubscribesTest()
        {
            var account = SendGridAccount.Parse(ConfigurationManager.ConnectionStrings["SendGrid"].ConnectionString);

            var client = account.CreateWebApiClient();

            var result = await client.Unsubscribes.GetAsync(new GetUnsubscribesParameter
            {
                Days = 10
            });

            await Task.WhenAll(result.Take(5).Select(x => client.Unsubscribes.DeleteAsync(new DeleteUnsubscribesParameter { Email = x.Email })));

            await client.Unsubscribes.AddAsync(new AddUnsubscribesParameter
            {
                Email = "hoge@example.com"
            });
        }

        private static async Task EnforcedTlsTest()
        {
            var account = SendGridAccount.Parse(ConfigurationManager.ConnectionStrings["SendGrid"].ConnectionString);

            var client = account.CreateWebApiV3Client();

            var result = await client.Settings.EnforcedTls.GetAsync();
        }
    }
}
