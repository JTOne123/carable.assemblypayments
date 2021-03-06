﻿using Newtonsoft.Json;
using Xunit;
using Carable.AssemblyPayments.Entities;
using Carable.AssemblyPayments.Implementations;
using Carable.AssemblyPayments.Internals;
using System;
using System.IO;
using Carable.AssemblyPayments.Abstractions;

namespace Carable.AssemblyPayments.Tests
{
    public class PayPalAccountTest : AbstractTest
    {
        [Fact]
        public void PayPalAccountDeserialization()
        {
            var jsonStr = "{ \"active\": true, \"created_at\": \"2015-04-25T12:31:39.324Z\", \"updated_at\": \"2015-04-25T12:31:39.324Z\", \"id\": \"70d93fe3-6c2e-4a1c-918f-13b8e7bb3779\", \"currency\": \"USD\", \"paypal\": { \"email\": \"test.me@promisepay.com\" }, \"links\": { \"self\": \"/paypal_accounts/70d93fe3-6c2e-4a1c-918f-13b8e7bb3779\", \"users\": \"/paypal_accounts/70d93fe3-6c2e-4a1c-918f-13b8e7bb3779/users\" } }";
            var payPalAccount = JsonConvert.DeserializeObject<PayPalAccount>(jsonStr);
            Assert.Equal("70d93fe3-6c2e-4a1c-918f-13b8e7bb3779", payPalAccount.Id);
            Assert.Equal("USD", payPalAccount.Currency);
            Assert.Equal("test.me@promisepay.com", payPalAccount.PayPal.Email);
        }

        [Fact]
        public void CreatePayPalAccountSuccessfully()
        {
            var content = Files.ReadAllText("./Fixtures/paypal_account_create.json");
            var client = GetMockClient(content);
            var repo = Get<IPayPalAccountRepository>(client.Object);

            var userId = "ec9bf096-c505-4bef-87f6-18822b9dbf2c"; //some user created before
            var account = new PayPalAccount
            {
                UserId = userId,
                Active = true,
                PayPal = new PayPal
                {
                    Email = "aaa@bbb.com"
                }
            };
            var createdAccount = repo.CreatePayPalAccount(account);
            Assert.NotNull(createdAccount);
            Assert.NotNull(createdAccount.Id);
            Assert.Equal("AUD", createdAccount.Currency); // It seems that currency is determined by country
            Assert.NotNull(createdAccount.CreatedAt);
            Assert.NotNull(createdAccount.UpdatedAt);

        }

        [Fact]
        public void GetPayPalAccountSuccessfully()
        {
            var id = "cd2ab053-25e5-491a-a5ec-0c32dbe76efa";
            var content = Files.ReadAllText("./Fixtures/paypal_account_create.json");
            var client = GetMockClient(content);
            var repo = Get<IPayPalAccountRepository>(client.Object);

            var gotAccount = repo.GetPayPalAccountById(id);

            Assert.Equal(id, gotAccount.Id);
        }

        [Fact]
        public void GetPayPalAccountEmptyId()
        {
            var client = GetMockClient("");

            var repo = Get<IPayPalAccountRepository>(client.Object);

            Assert.Throws<ArgumentException>(()=>repo.GetPayPalAccountById(string.Empty));
        }

        [Fact]
        public void GetUserForPayPalAccountSuccessfully()
        {
            var id = "3a780d4a-5de0-409c-9587-080930ddea3c";

            var content = Files.ReadAllText("./Fixtures/paypal_account_get_users.json");
            var client = GetMockClient(content);
            var repo = Get<IPayPalAccountRepository>(client.Object);

            var userId = "ec9bf096-c505-4bef-87f6-18822b9dbf2c"; //some user created before

            var gotUser = repo.GetUserForPayPalAccount(id);

            Assert.NotNull(gotUser);

            Assert.Equal(userId, gotUser.Id);
        }

        [Fact]
        public void DeletePayPalAccountSuccessfully()
        {
            var content = Files.ReadAllText("./Fixtures/paypal_account_delete.json");
            var client = GetMockClient(content);
            var repo = Get<IPayPalAccountRepository>(client.Object);

            var result = repo.DeletePayPalAccount("cd2ab053-25e5-491a-a5ec-0c32dbe76efa");
            Assert.True(result);
        }

    }
}
