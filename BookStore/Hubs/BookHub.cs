using BookStore.Model;
using Microsoft.AspNetCore.SignalR;

namespace BookStore.Hubs
{
    public class BookHub : Hub
    {
        public async Task SendBookUpdate(Book book)
        {
            await Clients.All.SendAsync("BookUpdated", book);
        }
    }
}