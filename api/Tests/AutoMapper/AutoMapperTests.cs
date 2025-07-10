using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Configuration;
using AutoMapper;
using Xunit;

namespace api.Tests.AutoMapper
{
    public class AutoMapperTests
    {
        [Fact]
        public void AutoMapper_Configuration_IsValid()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperConfigDTOs>();
                 cfg.AddProfile<AutoMapperConfigViewModels>();
            });

            config.AssertConfigurationIsValid();
        }
    }
}