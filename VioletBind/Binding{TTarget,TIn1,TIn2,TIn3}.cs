﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace VioletBind
{
    internal class Binding<TTarget, TIn1, TIn2, TIn3> : Binding<TTarget>
    {
        private readonly Func<TTarget, TIn1> _in1;
        private readonly Func<TTarget, TIn2> _in2;
        private readonly Func<TTarget, TIn3> _in3;
        private readonly Action<TTarget, TIn1, TIn2, TIn3> _setter;

        public Binding(Expression<Func<TTarget, TIn1>> in1, Expression<Func<TTarget, TIn2>> in2, Expression<Func<TTarget, TIn3>> in3, Action<TTarget, TIn1, TIn2, TIn3> setter, TTarget target)
            : base(target)
        {
            _in1 = in1.Compile();
            _in2 = in2.Compile();
            _in3 = in3.Compile();
            _setter = setter;
            Set();

            AddObservers(
                PropertyPaths<TTarget>.Get(in1)
                .Select(p => new PropertyPathObserver<TTarget>(p, target)));

            AddObservers(
                PropertyPaths<TTarget>.Get(in2)
                .Select(p => new PropertyPathObserver<TTarget>(p, target)));

            AddObservers(
                PropertyPaths<TTarget>.Get(in3)
                .Select(p => new PropertyPathObserver<TTarget>(p, target)));
        }

        internal override sealed void Set()
        {
            _setter(Target, _in1(Target), _in2(Target), _in3(Target));
        }
    }
}
