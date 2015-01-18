namespace Porter.Diagnostics.Decorator
{
	internal interface IClrRuntimeDecorator
	{
		IClrHeapDecorator GetHeap();
	}
}