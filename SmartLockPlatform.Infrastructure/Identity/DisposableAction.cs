using System.Reflection;

namespace SmartLockPlatform.Infrastructure.Identity;


/// <summary>
/// Provides a base implementation of a disposable object
/// </summary>
public abstract class Disposable : IDisposable
{
    private bool _disposed;
    protected virtual object This => this;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~Disposable() => Dispose(false);

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            DisposeExplicit();
        }

        DisposeImplicit();

        _disposed = true;
    }

    /// <summary>
    /// Performs operations when the instance is explicitly disposed.
    /// Free other managed objects that implement IDisposable only
    /// </summary>
    protected virtual void DisposeExplicit()
    {
    }

    /// <summary>
    /// Performs operations when the instance is implicitly disposed.
    /// Release any unmanaged objects set the object references to null
    /// </summary>
    protected virtual void DisposeImplicit()
    {
    }

    protected void ThrowIfDisposed()
    {
        if (!_disposed) return;

        var objectName = This.GetType().GetTypeInfo().Name;
        throw new ObjectDisposedException(objectName);
    }
}

/// <summary>
/// This class can be used to provide an action when
/// Dispose method is called.
/// </summary>
public class DisposableAction : Disposable
{
    private Action? _action;

    /// <summary>
    /// Creates a new <see cref="DisposableAction"/> object.
    /// </summary>
    /// <param name="action">Action to be executed when this object is disposed.</param>
    public DisposableAction(Action action)
    {
        _action = action;
    }

    protected override void DisposeExplicit()
    {
        // Interlocked prevents multiple execution of the _action.
        var action = Interlocked.Exchange(ref _action, null);
        action?.Invoke();
    }
}

/// <summary>
/// This class can be used to provide an action when
/// Dispose method is called.
/// </summary>
public class DisposableAction<T> : Disposable
    where T : DisposableAction<T>
{
    private Action<T>? _action;

    /// <summary>
    /// Creates a new <see cref="DisposableAction"/> object.
    /// </summary>
    /// <param name="action">Action to be executed when this object is disposed.</param>
    public DisposableAction(Action<T> action)
    {
        _action = action;
    }

    protected override void DisposeExplicit()
    {
        // Interlocked prevents multiple execution of the _action.
        var action = Interlocked.Exchange(ref _action, null);
        action?.Invoke((T)this);
    }
}