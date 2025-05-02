using System.Text.Json;
using System.Text.Json.Serialization;
using NotificationService.Domain.Entities;

namespace NotificationService.Infrastructure.Models;

internal class FirebaseCredential
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("project_id")]
    public string ProjectId { get; set; }

    [JsonPropertyName("private_key_id")]
    public string PrivateKeyId { get; set; }

    [JsonPropertyName("private_key")]
    public string PrivateKey { get; set; }

    [JsonPropertyName("client_email")]
    public string ClientEmail { get; set; }

    [JsonPropertyName("client_id")]
    public string ClientId { get; set; }

    [JsonPropertyName("auth_uri")]
    public string AuthUri { get; set; }

    [JsonPropertyName("token_uri")]
    public string TokenUri { get; set; }

    [JsonPropertyName("auth_provider_x509_cert_url")]
    public string AuthProviderX509CertUrl { get; set; }

    [JsonPropertyName("client_x509_cert_url")]
    public string ClientX509CertUrl { get; set; }

    [JsonPropertyName("universe_domain")]
    public string UniverseDomain { get; set; }

    public string ToJson() => JsonSerializer.Serialize(this);
    
    public static FirebaseCredential ConvertFrom(FirebaseSetting firebaseSetting)
    {
        return new()
        {
            Type = firebaseSetting.Type,
            ProjectId = firebaseSetting.ProjectId,
            PrivateKeyId = firebaseSetting.PrivateKeyId,
            PrivateKey = firebaseSetting.PrivateKey,
            ClientEmail = firebaseSetting.ClientEmail,
            ClientId = firebaseSetting.ClientId,
            AuthUri = firebaseSetting.AuthUri,
            TokenUri = firebaseSetting.TokenUri,
            AuthProviderX509CertUrl = firebaseSetting.AuthProviderX509CertUrl,
            ClientX509CertUrl = firebaseSetting.ClientX509CertUrl,
            UniverseDomain = firebaseSetting.UniverseDomain
        };
    }
}
