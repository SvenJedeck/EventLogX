# Introduction

<< EventLogSourceWizard >>

'------
-||- A small tool to create and delete/delete EventLogs and Sources in a safe way. -||-
'------

1) Project target

This project is a small showcase about designing code which is - independent from GUI - reuseable. 
For this reason this is a combined Console/WinForm project, in which you can start a WinForm and also use the base class by the shell. Both GUIs are using the main class "EventLogX".
- The CMD is using this class directly. (Don't look at spagetti code. It was written just in a very fast way to demonstrate the using of main class.)
- The WinForm is using a nearly API class to access the main class.
  There is no Interface-Class at the moment, because the main class should be more ordered and structured in a more common and clear way before implementing an interface.
  The inner structure of main class was not main goal of the project. It can follow later within the using of some smart events, which are missing totaly at this time. 
  The WinForm is a real ThinClint. The demonstration of a real small client was the big second target.

2) Project itself

  The solution delivers a small wizard to delete and create EventLogs and Sources in a safe and easy way. 
  The reason for the idea was to get some support while design time of any coding. Most time the logs/sources are deployed at installation time and the components for 
  creating this stuff is there, not in the main solution. By creating an eventlog you must follow some small pieces of rules given by framework and/or OS. And this wizard respects these rules.
  So there is no need for checking up the rules again, if a quick new source or log is needed. 
   
3) Good to know

There are some rules for creating rules in MSDN. E.g. 
- Unique shortnames (8 digits) for eventlogs. This doesn't fit any more with W10. You can use MyEventLog01 and MyEventLog02. 
- With W10 you do not need to restart machine after deleting log/source.
and so on.. ;)

! This tool is only tested on W10 !

'#################################################'
'### !! Tool must run with admin privilegs. !! ###
'#################################################'
That results in starting VS with privilegs. 
This is due to the fact, that Windows needs admin rights to loop sources (cus you need them to look into security log)

This tool doesn't support ressource files creating log/sources. 


# Getting Started
1.	Installation process

If you use Setup, then just install and start. 

2. How to use

Some infos needed to understand the tool.

Using from scretch:

- You can only delete logs/sources which you OWN for yourself. (Property: owner (Values: OWN, OTHER))
	This information is stored in registry (Computer\HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog\) "OwnLogs" and "OwnSources" (MULTI_SZ)

- You can add sources to OTHER logs (e.g. "Application") and you switch ownership from built in sources, but not W10 built in logs. 

- You can't add/edit/delete any property/item of "PROTECED_FULLY" logs. (System and Security log)

Using as programmer:

- If you want to protect some more logs in this way, you must edit following code in "EventlogsX":
	>> Private ThisProtectedLogs As New List(Of String)({"Security", "System", "Application", "HardwareEvents", "Internet Explorer", "Key Management Service", "OAlerts", "Windows Azure", "Windows PowerShell"}) <<
	Just add the LogName (not DisplayName) to the list.

- You can't add/edit/delete any property/item of "PROTECED_FULLY" logs. These are identified in list:
	>> Private ThisProtectedLogsFull As New List(Of String)({"Security", "System"}) <<
	As name said. They are fully protected.

If you want to use the pseudo API for a WinForm, you must refer/link some controls:

	- List/Combo for OTHER logs
	- List/Combo for OWN logs
	- List/Combo for OTHER sources
	- List/Combo for OWN sources
		- txt for selected/selfwritten log
		- txt for selected/selfwritten source
		OR
		- txt for selected log
		- txt for selected source
		- txt for selfwritten log
		- txt for selfwritten source

You can refer/link all action buttons. (See example WinForm: "FrmThinCLient"). As a result, you have a full functional tool with 1 line code.


3.	Software dependencies
None

4.	Latest releases
V1.2

# Build and Test
As descriped below, it was not my intention to have a class which appears in top proper and common way. Events and some clean structure are missing to reach that target. 
I know about..

Ignoring that topic, if you want to understand the main part of code logic you must take a look at "ActionState" and "Log/Source-State". 
The ActionState is derived from Log/Source-State. Depending on the ActionState you can carry an action into execution or even not. 
Thats a simple way to adapt any change requests. All assumpation logic is focused there. 

If you want to use the objects (logs/sources), you need to use LINQ, cus I placed all collection stuff in generic lists, which are most time public/friend.

# Contribute
Just hire me.
