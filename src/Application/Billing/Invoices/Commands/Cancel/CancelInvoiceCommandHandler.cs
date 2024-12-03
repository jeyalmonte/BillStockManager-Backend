using Domain.Billing.Repositories;
using SharedKernel.Interfaces;
using SharedKernel.Interfaces.Messaging;
using SharedKernel.Results;

namespace Application.Billing.Invoices.Commands.Cancel;
public class CancelInvoiceCommandHandler(
	IInvoiceRepository invoiceRepository,
	IUnitOfWork unitOfWork
	) : ICommandHandler<CancelInvoiceCommand, Success>
{
	public async Task<Result<Success>> Handle(CancelInvoiceCommand request, CancellationToken cancellationToken)
	{
		var invoice = await invoiceRepository.GetByIdAsync(
			id: request.Id,
			asNoTracking: false,
			cancellationToken: cancellationToken);

		if (invoice is null)
		{
			return Error.NotFound(description: "Invoice not found.");
		}

		var result = invoice.MarkAsCancelled();
		if (result.HasError)
		{
			return result.Errors;
		}
		await unitOfWork.CommitAsync(cancellationToken);

		return Result.Success;
	}
}
