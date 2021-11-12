// <copyright file="GenericResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAccess.Domains.Responses
{
    public class GenericResponse<T>
    {
        public T Data { get; set; }

        public int Count { get; set; }
    }
}
