using Microsoft.AspNetCore.Http;
using NotificationService.Application.Common.Models;
using NotificationService.Application.Contracts.DTOs.Responses;
using NotificationService.Application.Features.Senders.Dtos;
using NotificationService.SharedKernel.Interfaces;

namespace NotificationService.Application.Features.Senders.Commands.SendEmail;

public record SendEmailCommand(SendEmailRequestDto RequestDto, string Owner, List<IFormFile>? Attachments = null) 
    : ICommand<BaseResponse<NotificationSentResponseDto>>;