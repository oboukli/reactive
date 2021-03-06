﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class Min<TSource> : Producer<TSource, Min<TSource>._>
    {
        private readonly IObservable<TSource> _source;
        private readonly IComparer<TSource> _comparer;

        public Min(IObservable<TSource> source, IComparer<TSource> comparer)
        {
            _source = source;
            _comparer = comparer;
        }

        protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => default(TSource) == null ? (_)new Null(_comparer, observer, cancel) : new NonNull(_comparer, observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal abstract class _ : IdentitySink<TSource>
        {
            protected readonly IComparer<TSource> _comparer;

            public _(IComparer<TSource> comparer, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _comparer = comparer;
            }
        }

        private sealed class NonNull : _
        {
            private bool _hasValue;
            private TSource _lastValue;

            public NonNull(IComparer<TSource> comparer, IObserver<TSource> observer, IDisposable cancel)
                : base(comparer, observer, cancel)
            {
                _hasValue = false;
                _lastValue = default(TSource);
            }

            public override void OnNext(TSource value)
            {
                if (_hasValue)
                {
                    var comparison = 0;

                    try
                    {
                        comparison = _comparer.Compare(value, _lastValue);
                    }
                    catch (Exception ex)
                    {
                        ForwardOnError(ex);
                        return;
                    }

                    if (comparison < 0)
                    {
                        _lastValue = value;
                    }
                }
                else
                {
                    _hasValue = true;
                    _lastValue = value;
                }
            }

            public override void OnError(Exception error)
            {
                ForwardOnError(error);
            }

            public override void OnCompleted()
            {
                if (!_hasValue)
                {
                    ForwardOnError(new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
                }
                else
                {
                    ForwardOnNext(_lastValue);
                    ForwardOnCompleted();
                }
            }
        }

        private sealed class Null : _
        {
            private TSource _lastValue;

            public Null(IComparer<TSource> comparer, IObserver<TSource> observer, IDisposable cancel)
                : base(comparer, observer, cancel)
            {
                _lastValue = default(TSource);
            }

            public override void OnNext(TSource value)
            {
                if (value != null)
                {
                    if (_lastValue == null)
                    {
                        _lastValue = value;
                    }
                    else
                    {
                        var comparison = 0;

                        try
                        {
                            comparison = _comparer.Compare(value, _lastValue);
                        }
                        catch (Exception ex)
                        {
                            ForwardOnError(ex);
                            return;
                        }

                        if (comparison < 0)
                        {
                            _lastValue = value;
                        }
                    }
                }
            }

            public override void OnCompleted()
            {
                ForwardOnNext(_lastValue);
                ForwardOnCompleted();
            }
        }
    }

    internal sealed class MinDouble : Producer<double, MinDouble._>
    {
        private readonly IObservable<double> _source;

        public MinDouble(IObservable<double> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<double> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : IdentitySink<double>
        {
            private bool _hasValue;
            private double _lastValue;

            public _(IObserver<double> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _hasValue = false;
                _lastValue = default(double);
            }

            public override void OnNext(double value)
            {
                if (_hasValue)
                {
                    if (value < _lastValue || double.IsNaN(value))
                    {
                        _lastValue = value;
                    }
                }
                else
                {
                    _lastValue = value;
                    _hasValue = true;
                }
            }

            public override void OnCompleted()
            {
                if (!_hasValue)
                {
                    ForwardOnError(new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
                }
                else
                {
                    ForwardOnNext(_lastValue);
                    ForwardOnCompleted();
                }
            }
        }
    }

    internal sealed class MinSingle : Producer<float, MinSingle._>
    {
        private readonly IObservable<float> _source;

        public MinSingle(IObservable<float> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<float> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : IdentitySink<float>
        {
            private bool _hasValue;
            private float _lastValue;

            public _(IObserver<float> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _hasValue = false;
                _lastValue = default(float);
            }

            public override void OnNext(float value)
            {
                if (_hasValue)
                {
                    if (value < _lastValue || float.IsNaN(value))
                    {
                        _lastValue = value;
                    }
                }
                else
                {
                    _lastValue = value;
                    _hasValue = true;
                }
            }

            public override void OnCompleted()
            {
                if (!_hasValue)
                {
                    ForwardOnError(new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
                }
                else
                {
                    ForwardOnNext(_lastValue);
                    ForwardOnCompleted();
                }
            }
        }
    }

    internal sealed class MinDecimal : Producer<decimal, MinDecimal._>
    {
        private readonly IObservable<decimal> _source;

        public MinDecimal(IObservable<decimal> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<decimal> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : IdentitySink<decimal>
        {
            private bool _hasValue;
            private decimal _lastValue;

            public _(IObserver<decimal> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _hasValue = false;
                _lastValue = default(decimal);
            }

            public override void OnNext(decimal value)
            {
                if (_hasValue)
                {
                    if (value < _lastValue)
                    {
                        _lastValue = value;
                    }
                }
                else
                {
                    _lastValue = value;
                    _hasValue = true;
                }
            }

            public override void OnCompleted()
            {
                if (!_hasValue)
                {
                    ForwardOnError(new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
                }
                else
                {
                    ForwardOnNext(_lastValue);
                    ForwardOnCompleted();
                }
            }
        }
    }

    internal sealed class MinInt32 : Producer<int, MinInt32._>
    {
        private readonly IObservable<int> _source;

        public MinInt32(IObservable<int> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<int> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : IdentitySink<int>
        {
            private bool _hasValue;
            private int _lastValue;

            public _(IObserver<int> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _hasValue = false;
                _lastValue = default(int);
            }

            public override void OnNext(int value)
            {
                if (_hasValue)
                {
                    if (value < _lastValue)
                    {
                        _lastValue = value;
                    }
                }
                else
                {
                    _lastValue = value;
                    _hasValue = true;
                }
            }

            public override void OnCompleted()
            {
                if (!_hasValue)
                {
                    ForwardOnError(new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
                }
                else
                {
                    ForwardOnNext(_lastValue);
                    ForwardOnCompleted();
                }
            }
        }
    }

    internal sealed class MinInt64 : Producer<long, MinInt64._>
    {
        private readonly IObservable<long> _source;

        public MinInt64(IObservable<long> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<long> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : IdentitySink<long>
        {
            private bool _hasValue;
            private long _lastValue;

            public _(IObserver<long> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _hasValue = false;
                _lastValue = default(long);
            }

            public override void OnNext(long value)
            {
                if (_hasValue)
                {
                    if (value < _lastValue)
                    {
                        _lastValue = value;
                    }
                }
                else
                {
                    _lastValue = value;
                    _hasValue = true;
                }
            }

            public override void OnCompleted()
            {
                if (!_hasValue)
                {
                    ForwardOnError(new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
                }
                else
                {
                    ForwardOnNext(_lastValue);
                    ForwardOnCompleted();
                }
            }
        }
    }

    internal sealed class MinDoubleNullable : Producer<double?, MinDoubleNullable._>
    {
        private readonly IObservable<double?> _source;

        public MinDoubleNullable(IObservable<double?> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<double?> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : IdentitySink<double?>
        {
            private double? _lastValue;

            public _(IObserver<double?> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _lastValue = default(double?);
            }

            public override void OnNext(double? value)
            {
                if (!value.HasValue)
                    return;

                if (_lastValue.HasValue)
                {
                    if (value < _lastValue || double.IsNaN((double)value))
                    {
                        _lastValue = value;
                    }
                }
                else
                {
                    _lastValue = value;
                }
            }

            public override void OnCompleted()
            {
                ForwardOnNext(_lastValue);
                ForwardOnCompleted();
            }
        }
    }

    internal sealed class MinSingleNullable : Producer<float?, MinSingleNullable._>
    {
        private readonly IObservable<float?> _source;

        public MinSingleNullable(IObservable<float?> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<float?> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : IdentitySink<float?>
        {
            private float? _lastValue;

            public _(IObserver<float?> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _lastValue = default(float?);
            }

            public override void OnNext(float? value)
            {
                if (!value.HasValue)
                    return;

                if (_lastValue.HasValue)
                {
                    if (value < _lastValue || float.IsNaN((float)value))
                    {
                        _lastValue = value;
                    }
                }
                else
                {
                    _lastValue = value;
                }
            }

            public override void OnCompleted()
            {
                ForwardOnNext(_lastValue);
                ForwardOnCompleted();
            }
        }
    }

    internal sealed class MinDecimalNullable : Producer<decimal?, MinDecimalNullable._>
    {
        private readonly IObservable<decimal?> _source;

        public MinDecimalNullable(IObservable<decimal?> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<decimal?> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : IdentitySink<decimal?>
        {
            private decimal? _lastValue;

            public _(IObserver<decimal?> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _lastValue = default(decimal?);
            }

            public override void OnNext(decimal? value)
            {
                if (!value.HasValue)
                    return;

                if (_lastValue.HasValue)
                {
                    if (value < _lastValue)
                    {
                        _lastValue = value;
                    }
                }
                else
                {
                    _lastValue = value;
                }
            }

            public override void OnCompleted()
            {
                ForwardOnNext(_lastValue);
                ForwardOnCompleted();
            }
        }
    }

    internal sealed class MinInt32Nullable : Producer<int?, MinInt32Nullable._>
    {
        private readonly IObservable<int?> _source;

        public MinInt32Nullable(IObservable<int?> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<int?> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : IdentitySink<int?>
        {
            private int? _lastValue;

            public _(IObserver<int?> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _lastValue = default(int?);
            }

            public override void OnNext(int? value)
            {
                if (!value.HasValue)
                    return;

                if (_lastValue.HasValue)
                {
                    if (value < _lastValue)
                    {
                        _lastValue = value;
                    }
                }
                else
                {
                    _lastValue = value;
                }
            }

            public override void OnCompleted()
            {
                ForwardOnNext(_lastValue);
                ForwardOnCompleted();
            }
        }
    }

    internal sealed class MinInt64Nullable : Producer<long?, MinInt64Nullable._>
    {
        private readonly IObservable<long?> _source;

        public MinInt64Nullable(IObservable<long?> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<long?> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : IdentitySink<long?>
        {
            private long? _lastValue;

            public _(IObserver<long?> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _lastValue = default(long?);
            }

            public override void OnNext(long? value)
            {
                if (!value.HasValue)
                    return;

                if (_lastValue.HasValue)
                {
                    if (value < _lastValue)
                    {
                        _lastValue = value;
                    }
                }
                else
                {
                    _lastValue = value;
                }
            }

            public override void OnCompleted()
            {
                ForwardOnNext(_lastValue);
                ForwardOnCompleted();
            }
        }
    }
}
