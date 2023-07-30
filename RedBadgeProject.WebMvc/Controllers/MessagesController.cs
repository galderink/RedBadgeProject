using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedBadgeProject.Data;
using RedBadgeProject.Data.Entities;
using RedBadgeProject.Models.Messages;
using RedBadgeProject.Services.Messages;

namespace RedBadgeProject.WebMvc.Controllers;


public class MessagesController : Controller
{
    private readonly IMessagesService _messagesService;
    private readonly AppDbContext _dbContext;

    public MessagesController(AppDbContext dbContext, IMessagesService messagesService)
    {
        _dbContext = dbContext;
        _messagesService = messagesService;
    }

    public IActionResult Inbox()
{
    List<MessagesEntity> messages = _dbContext.GetMessagesFromDatabase();
    var messagesList = messages.Select(entity => new MessagesModel
    {
        Id = entity.Id,
        SenderId = entity.SenderId,
        ReceiverId = entity.ReceiverId,
        Subject = entity.Subject,
        Content = entity.Content,
        SentAt = entity.SentAt,
        SenderName = entity.SenderName
    }).ToList();

    return View(messagesList);
}


    public IActionResult CreateMessage()
    {
        return View();
    }

    [HttpPost]
public async Task<IActionResult> CreateMessage(MessagesModel message)
{
    if (ModelState.IsValid)
    {
        // Get the ReceiverId based on the recipient's name
        var recipient = await _dbContext.Users.FirstOrDefaultAsync(u => u.FirstName == message.RecipientName);
        if (recipient == null)
        {
            ModelState.AddModelError("", "Recipient not found.");
            return View(message);
        }

        message.ReceiverId = recipient.Id;

        var isMessageCreated = await _messagesService.CreateMessageAsync(message);

        if (isMessageCreated)
        {
            return RedirectToAction("Inbox", "Messages");
        }
        else
        {
            ModelState.AddModelError("", "Failed to create the message.");
            return View(message);
        }
    }

    return View(message);
}

}
