Imports MPXJ.Net

Module MpxjQuery

    ''' <summary>
    ''' Main entry point.
    ''' </summary>
    ''' <param name="args">command line arguments</param>
    Sub Main(args As String())
        Try
            If args.Length <> 1 Then
                Console.WriteLine("Usage: MpxQuery <input file name>")
            Else
                Query(args(0))
            End If

        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
        End Try

    End Sub

    ''' <summary>
    ''' This method performs a set of queries to retrieve information
    ''' from a schedule file.
    ''' </summary>
    ''' <param name="filename">name of the project file</param>
    Private Sub Query(filename As String)
        Dim reader = New UniversalProjectReader()
        Dim file As ProjectFile = reader.Read(filename)

        ListProjectHeader(file)

        ListResources(file)

        ListTasks(file)

        ListAssignments(file)

        ListAssignmentsByTask(file)

        ListAssignmentsByResource(file)

        ListHierarchy(file)

        ListTaskNotes(file)

        ListResourceNotes(file)

        ListRelationships(file)

        ListSlack(file)

        ListCalendars(file)
    End Sub

    ''' <summary>
    ''' Reads basic summary details from the project header.
    ''' </summary>
    ''' <param name="file">project file</param>
    Private Sub ListProjectHeader(file As ProjectFile)
        Dim header As ProjectProperties = file.ProjectProperties
        Dim formattedStartDate As String = If(header.StartDate Is Nothing, "(none)", header.StartDate.ToString())
        Dim formattedFinishDate As String = If(header.FinishDate Is Nothing, "(none)", header.FinishDate.ToString())

        Console.WriteLine("Project Header: StartDate=" & formattedStartDate & " FinishDate=" & formattedFinishDate)
        Console.WriteLine()
    End Sub

    ''' <summary>
    ''' This method lists all resources defined in the file.
    ''' </summary>
    ''' <param name="file">project file</param>
    Private Sub ListResources(file As ProjectFile)
        For Each resource As Resource In file.Resources
            Console.WriteLine("Resource: " & resource.Name & " (Unique ID=" & ToString(resource.UniqueID) & ") Start=" & ToString(resource.Start) & " Finish=" & ToString(resource.Finish))
        Next
        Console.WriteLine()
    End Sub

    ''' <summary>
    ''' This method lists all tasks defined in the file.
    ''' </summary>
    ''' <param name="file">project file</param>
    Private Sub ListTasks(file As ProjectFile)
        For Each task As Task In file.Tasks
            Dim startDate As String
            Dim finishDate As String
            Dim duration As String
            Dim dur As Duration

            Dim [date] = task.Start
            If [date] IsNot Nothing Then
                startDate = [date].ToString()
            Else
                startDate = "(no date supplied)"
            End If

            [date] = task.Finish
            If [date] IsNot Nothing Then
                finishDate = [date].ToString()
            Else
                finishDate = "(no date supplied)"
            End If

            dur = task.Duration
            If dur IsNot Nothing Then
                duration = dur.ToString()
            Else
                duration = "(no duration supplied)"
            End If

            Dim baselineDuration As String = task.BaselineDurationText
            If baselineDuration Is Nothing Then
                dur = task.BaselineDuration
                If dur IsNot Nothing Then
                    baselineDuration = dur.ToString()
                Else
                    baselineDuration = "(no duration supplied)"
                End If
            End If

            Console.WriteLine("Task: " & task.Name & " ID=" & ToString(task.ID) & " Unique ID=" & ToString(task.UniqueID) & " (Start Date=" & startDate & " Finish Date=" & finishDate & " Duration=" & duration & " Baseline Duration=" & baselineDuration & " Outline Level=" & ToString(task.OutlineLevel) & " Outline Number=" & task.OutlineNumber & " Recurring=" & task.Recurring & ")")
        Next
        Console.WriteLine()
    End Sub

    ''' <summary>
    ''' This method lists all tasks defined in the file in a hierarchical format, 
    ''' reflecting the parent-child relationships between them.
    ''' </summary>
    ''' <param name="file">project file</param>
    Private Sub ListHierarchy(file As ProjectFile)
        For Each task As Task In file.ChildTasks
            Console.WriteLine("Task: " & task.Name)
            ListHierarchy(task, " ")
        Next

        Console.WriteLine()
    End Sub

    ''' <summary>
    ''' Helper method called recursively to list child tasks.
    ''' </summary>
    ''' <param name="task">Task instance</param>
    ''' <param name="indent">print indent</param>
    Private Sub ListHierarchy(task As Task, indent As String)
        For Each child As Task In task.ChildTasks
            Console.WriteLine(indent & "Task: " & child.Name)
            ListHierarchy(child, indent & " ")
        Next
    End Sub

    ''' <summary>
    ''' This method lists all resource assignments defined in the file.
    ''' </summary>
    ''' <param name="file">project file</param>
    Private Sub ListAssignments(file As ProjectFile)
        Dim task As Task
        Dim resource As Resource
        Dim taskName As String
        Dim resourceName As String

        For Each assignment As ResourceAssignment In file.ResourceAssignments
            task = assignment.Task
            If task Is Nothing Then
                taskName = "(null task)"
            Else
                taskName = task.Name
            End If

            resource = assignment.Resource
            If resource Is Nothing Then
                resourceName = "(null resource)"
            Else
                resourceName = resource.Name
            End If

            Console.WriteLine("Assignment: Task=" & taskName & " Resource=" & resourceName)
        Next

        Console.WriteLine()
    End Sub

    ''' <summary>
    ''' This method displays the resource assignments for each task. 
    ''' This time rather than just iterating through the list of all 
    ''' assignments in the file, we extract the assignments on a task-by-task basis.
    ''' </summary>
    ''' <param name="file">project file</param>
    Private Sub ListAssignmentsByTask(file As ProjectFile)
        For Each task As Task In file.Tasks
            Console.WriteLine("Assignments for task " & task.Name & ":")

            For Each assignment As ResourceAssignment In task.ResourceAssignments
                Dim resource As Resource = assignment.Resource
                Dim resourceName As String

                If resource Is Nothing Then
                    resourceName = "(null resource)"
                Else
                    resourceName = resource.Name
                End If

                Console.WriteLine("   " & resourceName)
            Next
        Next

        Console.WriteLine()
    End Sub

    ''' <summary>
    ''' This method displays the resource assignments for each resource. 
    ''' This time rather than just iterating through the list of all 
    ''' assignments in the file, we extract the assignments on a resource-by-resource basis.
    ''' </summary>
    ''' <param name="file">project file</param>
    Private Sub ListAssignmentsByResource(file As ProjectFile)
        For Each resource As Resource In file.Resources
            Console.WriteLine("Assignments for resource " & resource.Name & ":")

            For Each assignment As ResourceAssignment In resource.TaskAssignments
                Dim task As Task = assignment.Task
                Console.WriteLine("   " & task.Name)
            Next
        Next

        Console.WriteLine()
    End Sub

    ''' <summary>
    ''' This method lists any notes attached to tasks.
    ''' </summary>
    ''' <param name="file">project file</param>
    Private Sub ListTaskNotes(file As ProjectFile)
        For Each task As Task In file.Tasks
            Dim notes As String = task.Notes

            If notes IsNot Nothing AndAlso notes.Length <> 0 Then
                Console.WriteLine("Notes for " & task.Name & ": " & notes)
            End If
        Next

        Console.WriteLine()
    End Sub

    ''' <summary>
    ''' This method lists any notes attached to resources.
    ''' </summary>
    ''' <param name="file">project file</param>
    Private Sub ListResourceNotes(file As ProjectFile)
        For Each resource As Resource In file.Resources
            Dim notes As String = resource.Notes

            If notes IsNot Nothing AndAlso notes.Length <> 0 Then
                Console.WriteLine("Notes for " & resource.Name & ": " & notes)
            End If
        Next

        Console.WriteLine()
    End Sub

    ''' <summary>
    ''' This method lists task predecessor and successor relationships.
    ''' </summary>
    ''' <param name="file">project file</param>
    Private Sub ListRelationships(file As ProjectFile)
        For Each task As Task In file.Tasks
            Console.Write(task.ID)
            Console.Write(ControlChars.Tab)
            Console.Write(task.Name)
            Console.Write(ControlChars.Tab)

            DumpRelationList(task.Predecessors, True)
            Console.Write(ControlChars.Tab)
            DumpRelationList(task.Successors, False)
            Console.WriteLine()
        Next
    End Sub

    ''' <summary>
    ''' Internal utility to dump relationship lists in a structured format that can 
    ''' easily be compared with the tabular data in MS Project.
    ''' </summary>
    ''' <param name="relations">project file</param>
    Private Sub DumpRelationList(relations As IList(Of Relation), predecessors as Boolean)
        If relations Is Nothing Or relations.Count = 0 Then
            Return
        End If

        If relations.Count > 1 Then
            Console.Write(""""c)
        End If

        Dim first = True
        For Each relation In relations
            If Not first Then
                Console.Write(","c)
            End If
            first = False
            If predecessors Then
                Console.Write(relation.PredecessorTask.ID)
            Else
                Console.Write(relation.SuccessorTask.ID)
            End If
            Dim lag As Duration = relation.Lag
            If Not relation.Type.Equals(RelationType.FinishStart) OrElse lag.DurationValue <> 0 Then
                Console.Write(relation.Type)
            End If

            If lag.DurationValue <> 0 Then
                If lag.DurationValue > 0 Then
                    Console.Write("+")
                End If
                Console.Write(lag)
            End If
        Next

        If relations.Count > 1 Then
            Console.Write(""""c)
        End If
    End Sub

    ''' <summary>
    ''' List the slack values for each task.
    ''' </summary>
    ''' <param name="file">project file</param>
    Private Sub ListSlack(file As ProjectFile)
        For Each task As Task In file.Tasks
            Console.WriteLine(task.Name & " Total Slack=" & ToString(task.TotalSlack) & " Start Slack=" & ToString(task.StartSlack) & " Finish Slack=" & ToString(task.FinishSlack))
        Next
    End Sub

    ''' <summary>
    ''' List details of all calendars in the file.
    ''' </summary>
    ''' <param name="file">project file</param>
    Private Sub ListCalendars(file As ProjectFile)
        For Each cal As ProjectCalendar In file.Calendars
            Console.WriteLine(cal.ToString())
        Next
    End Sub

    ''' <summary>
    ''' Convenience method to handle null values
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ToString(value As Integer?)
        Dim result As String
        If value Is Nothing Then
            result = "null"
        Else
            result = value.ToString()
        End If
        Return result
    End Function

    ''' <summary>
    ''' Convenience method to handle null values
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ToString(value As Date?)
        Dim result As String
        If value Is Nothing Then
            result = "null"
        Else
            result = value.ToString()
        End If
        Return result
    End Function

    ''' <summary>
    ''' Convenience method to handle null values
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ToString(value As Duration)
        Dim result As String
        If value Is Nothing Then
            result = "null"
        Else
            result = value.ToString()
        End If
        Return result
    End Function

End Module
