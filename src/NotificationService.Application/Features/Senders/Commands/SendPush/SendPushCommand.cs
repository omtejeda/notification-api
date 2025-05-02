using NotificationService.Application.Common.Models;
using NotificationService.Application.Contracts.DTOs.Responses;
using NotificationService.Application.Features.Senders.Dtos;
using NotificationService.SharedKernel.Interfaces;

namespace NotificationService.Application.Features.Senders.Commands.SendPush;

public record SendPushCommand(SendPushRequestDto RequestDto, string Owner) 
    : ICommand<BaseResponse<NotificationSentResponseDto>>;