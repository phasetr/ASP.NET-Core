using Amazon.CDK;
using Amazon.CDK.AWS.Route53;
using Amazon.CDK.AWS.SES;
using Constructs;

namespace CdkSes;

public class CdkSesStack : Stack
{
    internal CdkSesStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
    {
        const string suffix = "CdkSes";
        const string domainName = "academic-event.com";
        var hostedZone = new PublicHostedZone(this, $"{suffix}-HostedZone", new PublicHostedZoneProps
        {
            ZoneName = domainName
        });
        var identity = new EmailIdentity(this, $"{suffix}-EmailIdentity", new EmailIdentityProps
        {
            Identity = Identity.PublicHostedZone(hostedZone),
            MailFromDomain = $"bounce.{domainName}"
        });
        var txtRecord = new TxtRecord(this, $"{suffix}-DmarcRecord", new TxtRecordProps
        {
            Zone = hostedZone,
            RecordName = $"_dmarc.{domainName}",
            Values = new[] {$"v=DMARC1; p=none; rua=mailto:dmarcreports@{domainName}"},
            Ttl = Duration.Hours(1)
        });
        var emailTemplate = new CfnTemplate(this, $"{suffix}-EmailTemplate", new CfnTemplateProps
        {
            Template = new CfnTemplate.TemplateProperty
            {
                TemplateName = $"{suffix}-MyEmailTemplate",
                SubjectPart = suffix + ": Hello, {{name}}!",
                HtmlPart = "<p>Hello {{name}}!</p>",
                TextPart = suffix + ": Hello {{name}}! in text"
            }
        });
    }
}
