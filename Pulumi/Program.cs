using System.Collections.Generic;
using Pulumi;
using Pulumi.AzureNative.Authorization;
using Pulumi.AzureNative.KeyVault;
using Pulumi.AzureNative.KeyVault.Inputs;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage;
using Pulumi.AzureNative.Web;
using Pulumi.AzureNative.Web.Inputs;
using Deployment = Pulumi.Deployment;
using SkuArgs = Pulumi.AzureNative.Storage.Inputs.SkuArgs;
using SkuName = Pulumi.AzureNative.Storage.SkuName;

return await Deployment.RunAsync(static async () =>
{
    var stackName = Deployment.Instance.StackName;

    var resourceGroupName = $"acme-{stackName}";
    var resourceGroup = new ResourceGroup(resourceGroupName,
        new ResourceGroupArgs
        {
            ResourceGroupName = resourceGroupName,
            Location = "WestEurope",
            Tags = { { "Project", "Pulumi" } }
        });

    var storageName = $"acmeas{stackName}";
    var storageAccount = new StorageAccount(storageName, new StorageAccountArgs
    {
        AccountName = storageName,
        ResourceGroupName = resourceGroup.Name,
        Location = resourceGroup.Location,
        Sku = new SkuArgs
        {
            Name = SkuName.Standard_LRS
        },
        Kind = Kind.StorageV2,
        Tags = { { "Project", "Pulumi" } }
    });

    var planName = $"acme-asp-{stackName}";
    var appServicePlan = new AppServicePlan(planName, new AppServicePlanArgs
    {
        Name = planName,
        ResourceGroupName = resourceGroup.Name,
        Location = resourceGroup.Location,
        Sku = new SkuDescriptionArgs
        {
            Name = "B1",
            Tier = "Basic"
        },
        Tags = { { "Project", "Pulumi" } }
    });

    var appServiceName = $"acme-app-{stackName}";
    var appService = new WebApp(appServiceName,
        new WebAppArgs
        {
            Name = appServiceName,
            ResourceGroupName = resourceGroup.Name,
            Location = resourceGroup.Location,
            ServerFarmId = appServicePlan.Id,
            Identity =
                new ManagedServiceIdentityArgs
                {
                    Type = Pulumi.AzureNative.Web.ManagedServiceIdentityType.SystemAssigned
                },
            SiteConfig = new SiteConfigArgs
            {
                NetFrameworkVersion = "v8.0",
                AppSettings = { new NameValuePairArgs { Name = "DOTNET_VERSION", Value = "8.0" } }
            },
            Tags = { { "Project", "Pulumi" } }
        });

    var clientConfig = Output.Create(GetClientConfig.InvokeAsync());
    var tenantId = clientConfig.Apply(config => config.TenantId);

    var keyVaultName = $"acme-kv-{stackName}";
    var keyVault = new Vault(keyVaultName,
        new VaultArgs
        {
            VaultName = keyVaultName,
            ResourceGroupName = resourceGroup.Name,
            Location = resourceGroup.Location,
            Properties = new VaultPropertiesArgs
            {
                Sku = new Pulumi.AzureNative.KeyVault.Inputs.SkuArgs
                {
                    Family = SkuFamily.A, Name = Pulumi.AzureNative.KeyVault.SkuName.Standard
                },
                TenantId = tenantId,
                AccessPolicies =
                {
                    new AccessPolicyEntryArgs
                    {
                        TenantId = tenantId,
                        ObjectId = appService.Identity.Apply(identity => identity!.PrincipalId),
                        Permissions = new PermissionsArgs { Secrets = { "get" } }
                    }
                }
            },
            Tags = { { "Project", "Pulumi" } }
        });

    var storageAccountKeys = await ListStorageAccountKeys.InvokeAsync(new ListStorageAccountKeysArgs
    {
        ResourceGroupName = resourceGroupName,
        AccountName = storageName
    });

    var primaryConnectionString = Output.Format($"DefaultEndpointsProtocol=https;AccountName={storageName};AccountKey={storageAccountKeys.Keys[0].Value};EndpointSuffix=core.windows.net");

    var secret = new Secret("AccountStorageConnectionString",
        new Pulumi.AzureNative.KeyVault.SecretArgs
        {
            ResourceGroupName = resourceGroup.Name,
            VaultName = keyVault.Name,
            SecretName = "AccountStorageConnectionString",
            Properties = new SecretPropertiesArgs
            {
                Value = primaryConnectionString
            }
        });

    return new Dictionary<string, object?>
    {
        ["resourceGroupName"] = resourceGroup.Name,
        ["storageAccountName"] = storageAccount.Name,
        ["appServicePlanName"] = appServicePlan.Name,
        ["appServiceName"] = appService.Name,
        ["keyVaultName"] = keyVault.Name,
        ["secretName"] = secret.Name
    };
});
