// Run the samples

using MPXJ.Net;
using MpxjSamples;
using MpxjSamples.HowToConvert;

new ConvertMppToMpx().Convert("_TestData/Sample1.mpp", "Sample1.mpx");
new ConvertUniversal().Convert("_TestData/Sample1.mpp", FileFormat.MPX, "Sample1.mpx");

new AnonymizeAProject().Execute("_TestData/Sample1.mpp");
new BuildFieldDictionary().Execute();
new CalendarSamples().Execute();
new CustomFieldDefinitions().Execute("_TestData/Sample1.mpp");
new PredecessorsAndSuccessors().Execute("_TestData/Sample1.mpp");
new ProjectCalendarExceptionsToDates().Execute("_TestData/Sample1.mpp");
new ReadProjectProperties().Execute("_TestData/Sample1.mpp");
new ReadTaskFields().Execute("_TestData/Sample1.mpp");
new CreateTimephased().Execute("create-timephased.xml");
