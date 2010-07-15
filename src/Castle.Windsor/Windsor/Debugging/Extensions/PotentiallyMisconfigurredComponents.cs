﻿// Copyright 2004-2010 Castle Project - http://www.castleproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Castle.Windsor.Debugging.Extensions
{
	using System.Collections.Generic;
	using System.Linq;

	using Castle.MicroKernel;
	using Castle.MicroKernel.SubSystems.Naming;

	public class PotentiallyMisconfigurredComponents : IContainerDebuggerExtension
	{
		private INamingSubSystem naming;

		public IEnumerable<DebuggerViewItem> Attach()
		{
			var waitingComponents = naming.GetKey2Handler()
				.Where(h => h.Value.CurrentState == HandlerState.WaitingDependency)
				.ToArray();
			if (waitingComponents.Length == 0)
			{
				yield break;
			}
			yield return new DebuggerViewItem("Potentially Misconfigured Components", "Count = " + naming.ComponentCount,
			                                  new HandlersByKeyDictionaryDebuggerView(waitingComponents));
		}

		public void Init(IKernel kernel)
		{
			naming = kernel.GetSubSystem(SubSystemConstants.NamingKey) as INamingSubSystem;
		}
	}
}