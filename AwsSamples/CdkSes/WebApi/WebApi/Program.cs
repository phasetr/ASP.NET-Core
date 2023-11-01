using System.Net;
using System.Net.Mail;
using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "This is a GET.");
app.MapGet("/mail", () =>
{
    // コンソールで設定した適切なアドレスを設定する：サンドボックスではコンソールで設定したアドレスでしか送信できない
    const string fromAddr = "";
    // TODO：コンソールから取得した値で書き換える
    const string smtpUsername = "";
    const string smtpPassword = "";
    const string host = "email-smtp.ap-northeast-1.amazonaws.com";
    const int port = 587;
    // コンソールで設定した適切なアドレスを設定する：サンドボックスではコンソールで設定したアドレスにしか送信できない
    const string toAddr = "";
    const string subject = "SUBJECT: TEST";
    const string body = "BODY: This is a test email.";

    var message = new MailMessage();
    message.IsBodyHtml = false;
    message.From = new MailAddress(fromAddr, fromAddr);
    message.To.Add(new MailAddress(toAddr));
    message.Subject = subject;
    message.Body = body;

    using var client = new SmtpClient(host, port);
    client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
    client.EnableSsl = true;
    try
    {
        client.Send(message);
        Console.WriteLine("email sent");
    }
    catch (Exception ex)
    {
        Console.WriteLine("the email was not sent");
        Console.WriteLine(ex.Message);
    }
});
app.MapGet("/sdkmail", async () =>
{
    var region = RegionEndpoint.APNortheast1;
    // コンソールで設定した適切なアドレスを設定する：サンドボックスではコンソールで設定したアドレスでしか送信できない
    const string fromAddr = "";
    const string toAddr = "";
    // IAMから取得する
    const string awsAccessKey = "";
    const string awsSecretKey = "";

    using var client = new AmazonSimpleEmailServiceClient(awsAccessKey, awsSecretKey, region);

    // Create a send email message request
    var dest = new Destination(new List<string> {toAddr});
    var subject = new Content("Testing Amazon SES through the API");
    var textBody = new Content("This is a test message sent by Amazon SES from the AWS SDK for .NET.");
    var body = new Body(textBody);
    var message = new Message(subject, body);
    var request = new SendEmailRequest(fromAddr, dest, message);

    // Send the email to recipients via Amazon SES
    try
    {
        Console.WriteLine($"Sending message to {fromAddr}");
        var response = await client.SendEmailAsync(request);
        Console.WriteLine($"Response - HttpStatusCode: {response.HttpStatusCode}, MessageId: {response.MessageId}");
    }
    catch (Exception e)
    {
        Console.WriteLine("the email was not sent");
        Console.WriteLine(e.Message);
    }
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
