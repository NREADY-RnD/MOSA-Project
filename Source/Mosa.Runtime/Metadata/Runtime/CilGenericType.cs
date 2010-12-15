using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Mosa.Runtime.Metadata.Loader;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.Metadata.Runtime
{

	public class CilGenericType : RuntimeType
	{
		private readonly GenericInstSigType signature;

		private RuntimeType genericType;

		private SigType[] genericArguments;

		public CilGenericType(IModuleTypeSystem moduleTypeSystem, RuntimeType genericType, GenericInstSigType genericTypeInstanceSignature) :
			base(moduleTypeSystem, genericType.Token)
		{
			this.signature = genericTypeInstanceSignature;
			this.genericArguments = signature.GenericArguments;

			this.genericType = genericType;

			base.Attributes = genericType.Attributes;

			this.Methods = this.GetMethods();
			this.Fields = this.GetFields();
		}

		public SigType[] GenericArguments
		{
			get
			{
				return genericArguments;
			}
		}

		protected override string GetName()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("{0}<", genericType.Name);

			foreach (SigType sigType in genericArguments)
			{
				if (sigType.IsOpenGenericParameter)
				{
					sb.AppendFormat("<{0}>, ", sigType.ToString());
				}
				else
				{
					sb.AppendFormat("{0}, ", GetRuntimeTypeForSigType(sigType).FullName);
				}
			}

			sb.Length -= 2;
			sb.Append(">");

			return sb.ToString();
		}

		protected override string GetNamespace()
		{
			return genericType.Namespace;
		}

		protected override RuntimeType GetBaseType()
		{
			return genericType.BaseType;
		}

		private IList<RuntimeMethod> GetMethods()
		{
			List<RuntimeMethod> methods = new List<RuntimeMethod>();
			foreach (CilRuntimeMethod method in this.genericType.Methods)
			{
				MethodSignature signature = new MethodSignature(method.MetadataModule.Metadata, method.Signature.Token);

				signature.ApplyGenericType(this.genericArguments);

				RuntimeMethod genericInstanceMethod = new CilGenericMethod(moduleTypeSystem, method, signature, this);

				methods.Add(genericInstanceMethod);
			}

			return methods;
		}

		private IList<RuntimeField> GetFields()
		{
			List<RuntimeField> fields = new List<RuntimeField>();
			foreach (CilRuntimeField field in this.genericType.Fields)
			{
				FieldSignature signature = new FieldSignature(field.MetadataModule.Metadata, field.Signature.Token);

				signature.ApplyGenericType(this.genericArguments);

				CilGenericField genericInstanceField = new CilGenericField(moduleTypeSystem, field, signature, this);

				fields.Add(genericInstanceField);
			}

			return fields;
		}

		private RuntimeType GetRuntimeTypeForSigType(SigType sigType)
		{
			RuntimeType result = null;

			switch (sigType.Type)
			{
				case CilElementType.Class:
					Debug.Assert(sigType is TypeSigType, @"Failing to resolve VarSigType in GenericType.");
					result = moduleTypeSystem.GetType(((TypeSigType)sigType).Token);
					break;

				case CilElementType.ValueType:
					goto case CilElementType.Class;

				case CilElementType.Var:
					throw new NotImplementedException(@"Failing to resolve VarSigType in GenericType.");

				case CilElementType.MVar:
					throw new NotImplementedException(@"Failing to resolve VarMSigType in GenericType.");

				default:
					{
						BuiltInSigType builtIn = sigType as BuiltInSigType;
						if (builtIn != null)
						{
							result = moduleTypeSystem.TypeSystem.GetType(builtIn.TypeName + ", mscorlib");
						}
						else
						{
							throw new NotImplementedException(String.Format("SigType of CilElementType.{0} is not supported.", sigType.Type));
						}
						break;
					}
			}

			return result;
		}

		public override bool ContainsOpenGenericParameters
		{
			get
			{
				return signature.ContainsGenericParameters;
			}
		}

		protected override IList<RuntimeType> LoadInterfaces()
		{
			List<RuntimeType> result = new List<RuntimeType>();

			foreach (RuntimeType type in genericType.Interfaces)
			{
				if (!type.ContainsOpenGenericParameters)
				{
                    result.Add(type);
                    continue;
				}
				// find the enclosed type
				foreach (RuntimeType runtimetype in ModuleTypeSystem.GetAllTypes())
				{
					RuntimeType basegeneric = (type as CilGenericType).genericType;

					// find the enclosed type 
					// -- only needs to search generic type interfaces
					foreach (RuntimeType runtimetype in ModuleTypeSystem.GetAllTypes())
					{
						CilGenericType runtimetypegeneric = runtimetype as CilGenericType;
						if (runtimetypegeneric == null)
							continue;
						if (type != runtimetypegeneric.genericType)
							continue;
						// FIXME
						if (signature == runtimetypegeneric.signature)
						{
							CilGenericType runtimetypegeneric = runtimetype as CilGenericType;
							if (runtimetypegeneric != null)
							{
								if (basegeneric == runtimetypegeneric.genericType)
								{
									if (SigType.Equals(signature.GenericArguments, runtimetypegeneric.signature.GenericArguments))
									{
										result.Add(runtimetype);
										break;
									}
								}
							}
						}
					}
				}
			}

			return result.Count != 0 ? result : NoInterfaces;
		}


		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		public override bool Equals(RuntimeType other)
		{
			CilGenericType crt = other as CilGenericType;

			if (crt == null)
				return false;

			return 
				this.moduleTypeSystem == crt.moduleTypeSystem &&
				genericType == crt.genericType &&
				signature == crt.signature &&
				SigType.Equals(this.genericArguments, crt.genericArguments) &&
				base.Equals(other);
		}

	}
}