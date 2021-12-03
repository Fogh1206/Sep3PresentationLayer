﻿using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using Sep3Blazor.Model;

namespace Sep3Blazor.Data.UserData
{
    public class UserService : IUserService
    {
        private readonly String URL = "https://localhost:5004";

        public async Task<User> ValidateUser(string username, string password)
        {
            using var channel = GrpcChannel.ForAddress(URL);
            var client = new BusinessServer.BusinessServerClient(channel);
            try
            {
                var reply = await client.ValidateUserAsync(
                    new Request {Name = username, Type = password});
                Console.WriteLine("Greeting: " + reply.Message);
                User user = JsonSerializer.Deserialize<User>(reply.Message);
                return user;
            }
            catch (RpcException e)
            {
                Console.WriteLine(e.Status.Detail);
                Console.WriteLine(e.Status.StatusCode);
                Console.WriteLine((int) e.Status.StatusCode);
                return null;
            }
        }

        public async Task RegisterUser(User user)
        {
            using var channel = GrpcChannel.ForAddress(URL);
            var client = new BusinessServer.BusinessServerClient(channel);
            try
            {
                var reply = await client.RegisterUserAsync(
                    new RegisterRequest
                    {
                        Username = user.username, Password = user.password,
                        FirstName = user.firstName, LastName = user.lastName
                    });
                Console.WriteLine("Greeting: " + reply);
            }
            catch (RpcException e)
            {
                Console.WriteLine(e.Status.Detail);
                Console.WriteLine(e.Status.StatusCode);
                Console.WriteLine((int) e.Status.StatusCode);
            }
        }

        public async Task EditUser(int id, string newPassword)
        {
            using var channel = GrpcChannel.ForAddress(URL);
            var client = new BusinessServer.BusinessServerClient(channel);
            try
            {
                var reply = await client.EditUserAsync(new EditUserRequest
                    {Id = id, NewPassword = newPassword});
                Console.WriteLine("Greeting: " + reply);
            }
            catch (RpcException e)
            {
                Console.WriteLine(e.Status.Detail);
                Console.WriteLine(e.Status.StatusCode);
                Console.WriteLine((int) e.Status.StatusCode);
            }
        }

        public async Task<List<User>> GetUserList(string username)
        {
            using var channel = GrpcChannel.ForAddress(URL);
            var client = new BusinessServer.BusinessServerClient(channel);
            try
            {
                var reply = await client.GetUserAsync(
                    new GetUserRequest {Username = username});
                Console.WriteLine("User" + reply);
                return JsonSerializer.Deserialize<List<User>>(reply.Message);
            }
            catch (RpcException e)
            {
                Console.WriteLine(e.Status.Detail);
                Console.WriteLine(e.Status.StatusCode);
                Console.WriteLine((int) e.Status.StatusCode);
            }


            return null;
        }

        public async Task DeleteUser(int id)
        {
            using var channel = GrpcChannel.ForAddress(URL);
            var client = new BusinessServer.BusinessServerClient(channel);
            try
            {
                var reply = await client.DeleteUserAsync(
                    new UserRequest()
                    {
                        Id = id
                    }
                );
                Console.WriteLine("Greeting: " + reply.Message);
            }
            catch (RpcException e)
            {
                Console.WriteLine(e.Status.Detail);
                Console.WriteLine(e.Status.StatusCode);
                Console.WriteLine((int) e.Status.StatusCode);
            }
        }
    }
}