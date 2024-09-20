using Azure.Identity;
using Azure.Security.KeyVault.Certificates;
using System;
using System.Threading.Tasks;

public class CertService
{
    public Azure.Response<KeyVaultCertificateWithPolicy> Certificate { get; private set; }

    public CertService()
    {
        Console.WriteLine("CertService instantiated");

        var initCertTask = Task.Factory.StartNew(async () =>
        {
            await InitCert();
        });

        initCertTask.Wait();
    }

    public async Task InitCert()
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

        var client = new CertificateClient(new Uri(kvUri), new DefaultAzureCredential());

        Certificate = await client.GetCertificateAsync(certificateName);
    }

    public string ShowCertProps()
    {
        return $"Your certificate version is '{Certificate.Value.Properties.Version}'";
    }
}