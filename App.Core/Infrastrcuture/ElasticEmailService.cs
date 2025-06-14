using AppCore.Common;
using Microsoft.Extensions.Configuration;
using System.Xml;
using static ElasticEmailClient.Api;
using static ElasticEmailClient.ApiTypes;

namespace App.Core.Infrastrcuture;
public class EmailManager
{
    private readonly IConfiguration config;

    public string fromEmail { get; set; }
    public string fromName { get; set; }
    public string apiKey { get; set; }

    public EmailManager(IConfiguration config)
    {
        this.config = config;
        apiKey = config.GetSection("MailSettings:APIKey").Value;
        fromName = config.GetSection("MailSettings:FromName").Value;
        fromEmail = config.GetSection("MailSettings:FromEmail").Value;
    }
    public async Task<Result> SendEmail(string subject, string[] msgTo, string[] msgCC, string html)
    {


        ApiKey = apiKey;
        try
        {
            await ElasticEmailClient.Api.Email.SendAsync(subject, fromEmail, fromName, msgTo: msgTo, bodyHtml: html,msgCC: msgCC);
            return Result.Ok();

        }
        catch (Exception ex)
        {
            if (ex is ApplicationException)
            {
                return Result.Fail("Server didn't accept the request: " + ex.Message);
            }
            else
                return Result.Fail("Something unexpected happened: " + ex.Message);
        }
    }

}

