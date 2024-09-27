using Azure.Identity;
using Azure.Security.KeyVault.Certificates;
using System;
using System.Security.Cryptography.X509Certificates;

public class CertService
{
    public Azure.Response<KeyVaultCertificateWithPolicy> Certificate { get; private set; }

    public CertService()
    {
        Console.WriteLine("CertService instantiated");

        InitCert();
    }

    public void InitCert()
    {
        string certificateName = "RootCert5611";

        string keyVaultName = Environment.GetEnvironmentVariable("KEY_VAULT_NAME");
        var kvUri = "https://" + keyVaultName + ".vault.azure.net";

        var clientOptions = new CertificateClientOptions()
        {
            Retry =
                    {
                        Delay = TimeSpan.FromSeconds(2),
                        MaxDelay = TimeSpan.FromSeconds(16),
                        MaxRetries = 5,
                        Mode = Azure.Core.RetryMode.Exponential
                    }
        };

        var client = new CertificateClient(new Uri(kvUri), new DefaultAzureCredential(), clientOptions);

        Certificate = client.GetCertificate(certificateName);

        var response = Certificate.GetRawResponse();

        if (response.IsError)
        {
            Console.WriteLine($"Certificate Response Code:{response.Status} Reason:{response.ReasonPhrase}");
        }
    }

    public string ShowCertProps()
    {
        return $"Your certificate version is '{Certificate.Value.Properties.Version}'";
    }

    public X509Certificate2 GetCertificate()
    {
        var value = Certificate.Value;
        var cer = value.Cer;
        var cert = new X509Certificate2(cer);
        return cert;
    }
}