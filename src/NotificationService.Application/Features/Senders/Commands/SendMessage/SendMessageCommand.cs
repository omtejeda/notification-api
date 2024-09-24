using NotificationService.Application.Common.Models;
using NotificationService.Application.Contracts.ResponseDtos;
using NotificationService.Application.Features.Senders.Dtos;
using NotificationService.SharedKernel.Interfaces;

namespace NotificationService.Application.Features.Senders.Commands.SendMessage;

public record SendMessageCommand(SendMessageRequestDto RequestDto, string Owner) 
    : ICommand<BaseResponse<NotificationSentResponseDto>>;