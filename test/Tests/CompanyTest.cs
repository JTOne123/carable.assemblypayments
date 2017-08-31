﻿using Newtonsoft.Json;
using Xunit;
using Carable.AssemblyPayments.Entities;
using Carable.AssemblyPayments.Implementations;
using System.IO;
using System.Linq;
using Carable.AssemblyPayments.Abstractions;

namespace Carable.AssemblyPayments.Tests
{
    public class CompanyTest : AbstractTest
    {
        [Fact]
        public void CompanyDeserialization()
        {
            const string jsonStr = "{ \"legal_name\": \"Igor\", \"name\": null, \"id\": \"e466dfb4-f05c-4c7f-92a3-09a0a28c7af5\", \"related\": { \"address\": \"07ed45e5-bb9d-459f-bb7b-a02ecb38f443\" }, \"links\": { \"self\": \"/companies/e466dfb4-f05c-4c7f-92a3-09a0a28c7af5\" } }";
            var company = JsonConvert.DeserializeObject<Company>(jsonStr);
            Assert.NotNull(company);
            Assert.Equal("Igor", company.LegalName);
            Assert.Equal("e466dfb4-f05c-4c7f-92a3-09a0a28c7af5", company.Id);
        }

        [Fact]
        public void ListCompaniesSuccessfully()
        {
            var content = Files.ReadAllText("./Fixtures/companies_list.json");

            var client = GetMockClient(content);
            var repo = Get<ICompanyRepository>(client.Object);
            var companies = repo.ListCompanies();
            client.VerifyAll();
            Assert.NotNull(companies);
            Assert.True(companies.Any());
        }

        [Fact]
        public void GetCompanyByIdSuccessfully()
        {
            var content = Files.ReadAllText("./Fixtures/companies_get_by_id.json");

            var client = GetMockClient(content);
            var repo = Get<ICompanyRepository>(client.Object);
            var company = repo.GetCompanyById("e466dfb4-f05c-4c7f-92a3-09a0a28c7af5");
            client.VerifyAll();
            Assert.NotNull(company);
            Assert.Equal("e466dfb4-f05c-4c7f-92a3-09a0a28c7af5",company.Id);
        }

        [Fact]
        public void CreateCompanySuccessfully()
        {
            var content = Files.ReadAllText("./Fixtures/companies_create.json");

            var client = GetMockClient(content);
            var repo = Get<ICompanyRepository>(client.Object);
            var userId = "ec9bf096-c505-4bef-87f6-18822b9dbf2c";
            var createdCompany = repo.CreateCompany(new Company
            {
                LegalName = "Test company #1",
                Name = "Test company #1",
                Country = "AUS"
            }, userId);
            client.VerifyAll();
            Assert.NotNull(createdCompany);
            Assert.NotNull(createdCompany.Id);
        }

        [Fact]
        public void EditCompanySuccessfully()
        {
            var content = Files.ReadAllText("./Fixtures/companies_edit.json");

            var client = GetMockClient(content);
            var repo = Get<ICompanyRepository>(client.Object);
            var editedCompany = repo.EditCompany(new Company
            {
                Id = "739dcfc5-adf0-4a00-b639-b4e05922994d",
                LegalName = "Test company #2",
                Name = "Test company #2",
                Country = "AUS"
            });
            client.VerifyAll();
            Assert.Equal("Test company #2", editedCompany.Name);
        }


    }
}