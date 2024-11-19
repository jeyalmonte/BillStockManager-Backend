using MediatR;

namespace SharedKernel.Interfaces.Messaging;
public interface IEventHandler<TEvent> : INotificationHandler<TEvent> where TEvent : INotification;
