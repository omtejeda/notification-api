using Microsoft.AspNetCore.Http;
using NotificationService.Application.Common.Models;
using NotificationService.Application.Contracts.ResponseDtos;
using NotificationService.Application.Features.Senders.Dtos;
using NotificationService.SharedKernel.Interfaces;

namespace NotificationService.Application.Features.Senders.Commands.SendEmail;

public record SendEmailCommand : ICommand<BaseResponse<NotificationSentResponseDto>>
{
    public SendEmailCommand(SendEmailRequestDto requestDto, string owner, List<IFormFile>? attachments = null)
    {
        RequestDto = requestDto;
        Owner = owner;
        Attachments = attachments;
    }

    public SendEmailRequestDto RequestDto { get; init; }
    public string Owner { get; init; }
    public List<IFormFile>? Attachments { get; init; }

}