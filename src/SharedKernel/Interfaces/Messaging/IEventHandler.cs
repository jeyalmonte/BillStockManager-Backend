using MediatR;

namespace SharedKernel.Interfaces.Messaging;
public interface IEventHandler<in TEvent> : INotificationHandler<TEvent> where TEvent : INotification;
