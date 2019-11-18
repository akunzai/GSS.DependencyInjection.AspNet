using System;

namespace GSS.DependencyInjection.Testing.Fakes
{
    public class FakeService : IFakeService,IDisposable
    {
        public bool Disposed { get; private set; }

        public void Dispose()
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(nameof(FakeService));
            }

            Disposed = true;
        }
    }
}
