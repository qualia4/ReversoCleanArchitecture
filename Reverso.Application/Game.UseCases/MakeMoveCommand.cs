using MediatR;
using Reverso.Domain.Web;

namespace Application;

public class MakeMoveCommand: IRequest<MakeMoveResult>
{
    public string Username { get; set; }
    public int CoordinateX { get; set; }
    public int CoordinateY { get; set; }
}

public class MakeMoveResult
{
    public string Message { get; set; }
}

public class MakeMoveCommandUseCase : IRequestHandler<MakeMoveCommand, MakeMoveResult>
{
    public async Task<MakeMoveResult> Handle(MakeMoveCommand request, CancellationToken cancellationToken)
    {
        await GlobalResources.EndpointListener.InvokeEvent((request.CoordinateX, request.CoordinateY), request.Username);
        return new MakeMoveResult(){Message = "Request sent"};
    }
}