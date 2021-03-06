﻿using System;
using System.Net;
using System.Text.Json;
using AWSECS.ContainerMetadata.Contracts;
using AWSECS.ContainerMetadata.Models;
using Microsoft.Extensions.Logging;

namespace AWSECS.ContainerMetadata.Services
{
    public class AWSContainerMetadata : IAWSContainerMetadata
    {
        private readonly IAWSContainerMetadataClient _awsEcsContainerMetadataClient;
        private readonly ILogger _logger;

        public AWSContainerMetadata(IAWSContainerMetadataClient awsEcsContainerMetadataClient, ILogger<AWSContainerMetadata> logger)
        {
            _awsEcsContainerMetadataClient = awsEcsContainerMetadataClient ?? throw new ArgumentNullException(nameof(awsEcsContainerMetadataClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public ContainerResponse GetContainerMetadata()
        {
            try
            {
                var containerMetadataString = _awsEcsContainerMetadataClient.GetContainerMetadata();

                if (!string.IsNullOrEmpty(containerMetadataString))
                {
                    return JsonSerializer.Deserialize<ContainerResponse>(containerMetadataString);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get AWS ECS container metadata.");
            }

            return default;
        }

        public IPAddress GetHostPrivateIPv4Address()
        {
            try
            {
                var ipAddressString = _awsEcsContainerMetadataClient.GetHostPrivateIPv4Address();

                if (IPAddress.TryParse(ipAddressString, out var ipAddress))
                {
                    return ipAddress;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get AWS ECS container host's private IPv4 address.");
            }

            return default;
        }

        public IPAddress GetHostPublicIPv4Address()
        {
            try
            {
                var ipAddressString = _awsEcsContainerMetadataClient.GetHostPublicIPv4Address();

                if (IPAddress.TryParse(ipAddressString, out var ipAddress))
                {
                    return ipAddress;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get AWS ECS container host's public IPv4 address.");
            }

            return default;
        }
    }
}
