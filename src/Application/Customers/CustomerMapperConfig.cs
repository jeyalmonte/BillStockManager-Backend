﻿using Domain.Customers;
using Mapster;
using SharedKernel.Contracts.Customers;

namespace Application.Customers;
public class CustomerMapperConfig
{
    public static void Configure()
    {
        TypeAdapterConfig<Customer, CustomerResponse>
            .NewConfig()
            .Map(dest => dest.Gender, src => src.Gender.ToString())
            .Map(dest => dest.DocumentType, src => src.DocumentType.ToString());
    }
}
