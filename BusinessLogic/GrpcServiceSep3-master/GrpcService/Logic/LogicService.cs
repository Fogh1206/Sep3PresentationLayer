﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcService.Model;

namespace GrpcService.Logic
{
    public class LogicService : ILogicService
    {
        private string uri = "http://localhost:8080";
        private readonly HttpClient _client;

        public LogicService()
        {
            _client = new HttpClient();
        }

        public async Task<Reply> PostGroup(PostGroupRequest request, ServerCallContext context)
        {
            HttpContent content = new StringContent(request.GroupName, Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await _client.PutAsync(uri + "/group/" + request.MemberId, content);
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception(responseMessage.StatusCode + responseMessage.ReasonPhrase);
            }

            string message = await responseMessage.Content.ReadAsStringAsync();
            Console.WriteLine(message);
            return await Task.FromResult(new Reply
            {
                Message = message
            });
        }

        public async Task<Reply> GetGroup(Request request, ServerCallContext context)
        {
            Console.WriteLine(request);
            HttpResponseMessage responseMessage = await _client.GetAsync(uri + "/group/" + request.Name);
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception("Error" + responseMessage.StatusCode + responseMessage.ReasonPhrase);
            }

            string message = await responseMessage.Content.ReadAsStringAsync();
            return await Task.FromResult(new Reply
            {
                Message = message
            });
        }

        public async Task<Reply> DeleteGroup(Request request, ServerCallContext context)
        {
            Console.WriteLine(request);
            HttpResponseMessage responseMessage = await _client.DeleteAsync(uri + "/group/" + request.Name);
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception("Error " + responseMessage.StatusCode + " " + responseMessage.ReasonPhrase);
            }

            string message = await responseMessage.Content.ReadAsStringAsync();
            Console.WriteLine(message);
            return await Task.FromResult(new Reply
            {
                Message = message
            });
        }

        public async Task<Reply> PostNote(NoteRequest request, ServerCallContext context)
        {
            Note note = new Note(request.NoteId, request.GroupId,
                request.Week, request.Year, request.Name, request.Status, request.Text);
            string str = JsonSerializer.Serialize(note);
            HttpContent content = new StringContent(str, Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await _client.PostAsync(uri + "/note", content);
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception("Error " + responseMessage.StatusCode + " " + responseMessage.ReasonPhrase);
            }

            string message = await responseMessage.Content.ReadAsStringAsync();
            return await Task.FromResult(new Reply
            {
                Message = message
            });
        }

        public async Task<Reply> PutNote(NoteRequest request, ServerCallContext context)
        {
            Note note = new Note(request.NoteId, request.GroupId,
                request.Week, request.Year, request.Name, request.Status, request.Text);
            string str = JsonSerializer.Serialize(note);
            HttpContent content = new StringContent(str, Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await _client.PutAsync(uri + "/note", content);
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception("Error " + responseMessage.StatusCode + " " + responseMessage.ReasonPhrase);
            }

            string message = await responseMessage.Content.ReadAsStringAsync();
            return await Task.FromResult(new Reply
            {
                Message = message
            });
        }

        public async Task<Reply> GetNoteList(Request request, ServerCallContext context)
        {
            Console.WriteLine(request.Name);
            HttpResponseMessage responseMessage = await _client.GetAsync(uri + "/note/" + request.Name);
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception(responseMessage.StatusCode + responseMessage.ReasonPhrase);
            }

            string message = await responseMessage.Content.ReadAsStringAsync();
            return await Task.FromResult(new Reply
            {
                Message = message
            });
        }

        public async Task<Reply> DeleteNote(Request request, ServerCallContext context)
        {
            Console.WriteLine("Name " + request);
            HttpResponseMessage responseMessage = await _client.DeleteAsync(uri + "/note/" + request.Name);
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception("Error " + responseMessage.StatusCode + " " + responseMessage.ReasonPhrase);
            }

            string message = await responseMessage.Content.ReadAsStringAsync();
            Console.WriteLine(message);
            return await Task.FromResult(new Reply
            {
                Message = message
            });
        }


        public async Task<RegisterReply> RegisterUser(RegisterRequest request, ServerCallContext context)
        {
            User temp = new User(0, request.FirstName, request.LastName, request.Username, request.Password);
            string str = JsonSerializer.Serialize(temp);
            HttpContent content = new StringContent(str, Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await _client.PostAsync(uri + "/unregisteruser", content);
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception("Error " + responseMessage.StatusCode + " " + responseMessage.ReasonPhrase);
            }

            string message = await responseMessage.Content.ReadAsStringAsync();
            Console.WriteLine(message);
            return await Task.FromResult(new RegisterReply
            {
                Message = message
            });
        }


        public async Task<Reply> ValidateUser(Request request, ServerCallContext context)
        {
            User temp = new User(0, "", "", request.Name, request.Type);
            string str = JsonSerializer.Serialize(temp);
            HttpContent content = new StringContent(str, Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await _client.PostAsync(uri + "/user", content);
            try
            {
                if (responseMessage.IsSuccessStatusCode)
                {
                    string message = await responseMessage.Content.ReadAsStringAsync();
                    Console.WriteLine(message);
                    User user = JsonSerializer.Deserialize<User>(message);
                    if (request.Type.Equals(user.password))
                    {
                        return await Task.FromResult(new Reply
                        {
                            Message = message
                        });
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

            return null;
        }

        public async Task<Reply> EditUser(EditUserRequest request, ServerCallContext context)
        {
            User temp = new User(request.Id, "", "", "", request.NewPassword);
            string str = JsonSerializer.Serialize(temp);
            HttpContent content = new StringContent(str, Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await _client.PostAsync(uri + $"/user/{request.Id}", content);
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception("Error " + responseMessage.StatusCode + " " + responseMessage.ReasonPhrase);
            }

            string message = await responseMessage.Content.ReadAsStringAsync();
            return await Task.FromResult(new Reply
            {
                Message = message
            });
        }

        public async Task<Reply> GetInvitationList(Request request, ServerCallContext context)
        {
            HttpResponseMessage responseMessage = await _client.GetAsync(uri + "/invitation/" + request.Name);
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception("Error " + responseMessage.StatusCode + " " + " " + responseMessage.ReasonPhrase);
            }

            string message = await responseMessage.Content.ReadAsStringAsync();
            return await Task.FromResult(new Reply
            {
                Message = message
            });
        }

        public async Task<Reply> PostInvitation(PostInvitationRequest request, ServerCallContext context)
        {
            Invitation invitation = new Invitation(request.Id, request.GroupId, null, request.InviteeId, null,
                request.InvitorId, null);
            string str = JsonSerializer.Serialize(invitation);
            HttpContent content = new StringContent(str, Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await _client.PostAsync(uri + "/invitation", content);
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception("Error " + responseMessage.StatusCode + " " + " " + responseMessage.ReasonPhrase);
            }

            string message = await responseMessage.Content.ReadAsStringAsync();
            return await Task.FromResult(new Reply
            {
                Message = message
            });
        }

        public async Task<Reply> DeleteInvitation(Request request, ServerCallContext context)
        {
            HttpResponseMessage responseMessage = await _client.DeleteAsync(uri + "/invitation/" + request.Name);
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception("Error " + responseMessage.StatusCode + " " + " " + responseMessage.ReasonPhrase);
            }

            string message = await responseMessage.Content.ReadAsStringAsync();
            return await Task.FromResult(new Reply
            {
                Message = message
            });
        }

        public async Task<Reply> GetUserList(Request request, ServerCallContext context)
        {
            Console.WriteLine(request.Name);
            HttpResponseMessage responseMessage = await _client.GetAsync(uri + "/userlist/" + request.Name);
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception("Error " + responseMessage.StatusCode + " " + " " + responseMessage.ReasonPhrase);
            }

            string message = await responseMessage.Content.ReadAsStringAsync();
            return await Task.FromResult(new Reply
            {
                Message = message
            });
        }


        public async Task<Reply> DeleteGroupMember(UserRequest request, ServerCallContext context)
        {
            HttpResponseMessage responseMessage = await _client.DeleteAsync(uri + "/groupmembers/" + request.Id);
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception("Error " + responseMessage.StatusCode + " " + " " + responseMessage.ReasonPhrase);
            }

            string message = await responseMessage.Content.ReadAsStringAsync();
            Console.WriteLine(message);
            return await Task.FromResult(new Reply
            {
                Message = message
            });
        }

        public async Task<Reply> DeleteUser(UserRequest request, ServerCallContext context)
        {
            Console.WriteLine("aleoo");
            HttpResponseMessage responseMessage = await _client.DeleteAsync(uri + "/user/" + request.Id);
            Console.WriteLine("aleoox2");
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception("Error " + responseMessage.StatusCode + " " + " " + responseMessage.ReasonPhrase);
            }
            Console.WriteLine("aleoox3");
            string message = await responseMessage.Content.ReadAsStringAsync();
            Console.WriteLine(message);
            return await Task.FromResult(new Reply
            {
                Message = message
            });
        }

        public async Task<Reply> GetUser(GetUserRequest request, ServerCallContext context)
        {
            HttpResponseMessage responseMessage = await _client.GetAsync(uri + "/users/" + request.Username);
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception("Error " + responseMessage.StatusCode + " " + " " + responseMessage.ReasonPhrase);
            }

            string message = await responseMessage.Content.ReadAsStringAsync();
            return await Task.FromResult(new Reply
            {
                Message = message
            });
        }

        public async Task<Reply> LeaveGroup(DeleteGroupMemberRequest request, ServerCallContext context)
        {
            HttpContent content = new StringContent(request.UserId.ToString(), Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage =
                await _client.PostAsync(uri + $"/groupmembers/{request.GroupId}", content);
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception("Error " + responseMessage.StatusCode + " " + " " + responseMessage.ReasonPhrase);
            }
            string message = await responseMessage.Content.ReadAsStringAsync();
            Console.WriteLine(message);
            return await Task.FromResult(new Reply
            {
                Message = message
            });
        }

    

        public async Task<Reply> GetGroupMembersList(Request request, ServerCallContext context)
        {
            Console.WriteLine(request.Name);
            HttpResponseMessage responseMessage = await _client.GetAsync(uri + "/groupmemberslist/" + request.Name);
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception("Error " + responseMessage.StatusCode + " " + " " + responseMessage.ReasonPhrase);
            }
            string message = await responseMessage.Content.ReadAsStringAsync();
            return await Task.FromResult(new Reply
            {
                Message = message
            });
        }

        public async Task<Reply> AddGroupMember(AddGroupMemberRequest request, ServerCallContext context)
        {
            Console.WriteLine(request.GroupId + ":" + request.UserId);
            GroupMembers temp = new GroupMembers(0, request.GroupId, null, request.UserId);
            string str = JsonSerializer.Serialize(temp);
            HttpContent content = new StringContent(str, Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await _client.PostAsync(uri + "/groupmembers", content);
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception("Error " + responseMessage.StatusCode + " " + " " + responseMessage.ReasonPhrase);
            }

            string message = await responseMessage.Content.ReadAsStringAsync();
            return await Task.FromResult(new Reply()
            {
                Message = message
            });
        }
    }
}