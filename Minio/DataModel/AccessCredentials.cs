/*
 * MinIO .NET Library for Amazon S3 Compatible Cloud Storage,
 * (C) 2021 MinIO, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Xml.Serialization;

namespace Minio.DataModel;

[Serializable]
[XmlRoot(ElementName = "Credentials")]
public class AccessCredentials
{
    public AccessCredentials(string accessKey, string secretKey,
        string sessionToken, DateTime expiration)
    {
        if (!string.IsNullOrEmpty(accessKey) || !string.IsNullOrEmpty(secretKey) || !string.IsNullOrEmpty(sessionToken))
        {
            AccessKey = accessKey;
            SecretKey = secretKey;
            SessionToken = sessionToken;
            Expiration = expiration.Equals(default) ? null : Utils.To8601String(expiration);
        }
        else
        {
            if (string.IsNullOrEmpty(accessKey))
                throw new ArgumentException($"'{nameof(accessKey)}' cannot be null or empty.", nameof(accessKey));

            if (string.IsNullOrEmpty(secretKey))
                throw new ArgumentException($"'{nameof(secretKey)}' cannot be null or empty.", nameof(secretKey));

            if (string.IsNullOrEmpty(sessionToken))
                throw new ArgumentException($"'{nameof(sessionToken)}' cannot be null or empty.", nameof(sessionToken));
        }
    }

    public AccessCredentials()
    {
    }

    [XmlElement(ElementName = "AccessKeyId", IsNullable = true)]
    public string AccessKey { get; set; }

    [XmlElement(ElementName = "SecretAccessKey", IsNullable = true)]
    public string SecretKey { get; set; }

    [XmlElement(ElementName = "SessionToken", IsNullable = true)]
    public string SessionToken { get; set; }

    // Needs to be stored in ISO8601 format from Datetime
    [XmlElement(ElementName = "Expiration", IsNullable = true)]
    public string Expiration { get; set; }

    public bool AreExpired()
    {
        if (string.IsNullOrWhiteSpace(Expiration)) return false;
        var expiry = Utils.From8601String(Expiration);
        return DateTime.Now.CompareTo(expiry) > 0;
    }
}