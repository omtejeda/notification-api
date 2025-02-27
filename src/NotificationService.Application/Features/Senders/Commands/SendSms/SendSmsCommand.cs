using NotificationService.Application.Common.Models;
using NotificationService.Application.Contracts.DTOs.Responses;
using NotificationService.Application.Features.Senders.Dtos;
using NotificationService.SharedKernel.Interfaces;

namespace NotificationService.Application.Features.Senders.Commands.SendSms;

public record SendSmsCommand(SendSmsRequestDto RequestDto, string Owner) 
    : ICommand<BaseResponse<NotificationSentResponseDto>>;