/*****************************************************************************/
/* This document and its contents are the property of Bombardier
 * Inc or its subsidiaries.  This document contains confidential
 * proprietary information.  The reproduction, distribution,
 * utilization or the communication of this document or any part
 * thereof, without express authorization is strictly prohibited.
 * Offenders will be held liable for the payment of damages.
 *
 * (C) 2016, Bombardier Inc. or its subsidiaries.  All rights reserved.
 *
 * Project      :  RTDM (Windows Application DLL)
 *//**
 * \file RtdmFileIO.c
 *//*
 *
 * Revision: 01SEP2016 - D.Smail : Original Release
 *		Mod: 20DEC2016 - D.Smail : Added function BuildDanFileName() so that
 *                                 filename could be created and returned
 *                                 to C# application without building a DAN file.
 *****************************************************************************/

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <io.h>
#include "MyTypes.h"
#include "MyFuncs.h"

#include "RtdmUtils.h"
#include "RtdmCrc32.h"
/*******************************************************************
 *
 *     C  O  N  S  T  A  N  T  S
 *
 *******************************************************************/
/* Drive and directory where downloaded files are stored and processed */
#define DRIVE_NAME                          "C:\\"
#define DIRECTORY_NAME                      "Ptu\\Pcu\\Data\\Temp\\"

#define MAX_NUMBER_OF_STREAM_FILES          (UINT16)(100)

#define FILE_READ_BUFFER_SIZE               1024
#define INVALID_FILE_INDEX                  0xFFFF
#define INVALID_TIME_STAMP                  0xFFFFFFFF

/*******************************************************************
 *
 *     E  N  U  M  S
 *
 *******************************************************************/
/** @brief */
typedef enum
{
    /** */
    OLDEST_TIMESTAMP,
    /** */
    NEWEST_TIMESTAMP
} TimeStampAge;

/*******************************************************************
 *
 *    S  T  R  U  C  T  S 
 *
 *    IMPORTANT: Structures must be packed to byte boundary. Default
 *    for Windows DLL is not byte boundary. Project property C/C++...
 *    Code Generation...Struct Member Alignment must be set to 1 Byte (/Zp1)
 *
 *******************************************************************/
/** @brief Preamble of RTDM header, CRC IS NOT calculated on this section of data */
typedef struct
{
    /** */
    char delimiter[4];
    /** */
    UINT8 endianness;
    /** */
    UINT16 headerSize;
    /** */
    UINT32 headerChecksum;
} RtdmHeaderPreambleStr;

/** @brief Postamble of RTDM header, CRC IS calculated on this section of data ONLY */
typedef struct
{
    /** */
    UINT8 headerVersion;
    /** */
    char consistId[16];
    /** */
    char carId[16];
    /** */
    char deviceId[16];
    /** */
    UINT16 dataRecordId;
    /** */
    UINT16 dataRecordVersion;
    /** */
    UINT32 firstTimeStampSecs;
    /** */
    UINT16 firstTimeStampMsecs;
    /** */
    UINT32 lastTimeStampSecs;
    /** */
    UINT16 lastTimeStampMsecs;
    /** */
    UINT32 numStreams;

} RtdmHeaderPostambleStr;

/** @brief Structure to contain variables in the RTDM header of the message */
typedef struct
{
    /** */
    RtdmHeaderPreambleStr preamble;
    /** */
    RtdmHeaderPostambleStr postamble;
} RtdmHeaderStr;

/*******************************************************************
 *
 *    S  T  A  T  I  C      V  A  R  I  A  B  L  E  S
 *
 *******************************************************************/
/** @brief */
static UINT16 m_ValidStreamFileListIndexes[MAX_NUMBER_OF_STREAM_FILES ];
/** @brief */
static UINT32 m_ValidTimeStampList[MAX_NUMBER_OF_STREAM_FILES ];
/** @brief */
static const char *m_StreamHeaderDelimiter = "STRM";
/** @brief */
static char m_ErrorMessage[256];

/*******************************************************************
 *
 *    S  T  A  T  I  C      F  U  N  C  T  I  O  N  S
 *
 *******************************************************************/
static INT32 PopulateValidStreamFileList (void);
static void PopulateValidFileTimeStamps (void);
static void SortValidFileTimeStamps (void);
static UINT16 GetNewestStreamFileIndex (void);
static UINT16 GetOldestStreamFileIndex (void);
static INT32 CreateViewableStreamFileName (FILE **ptr, char **viewableStreamFileName);
static INT32 IncludeXMLFile (FILE *filePtr);
static INT32 IncludeRTDMHeader (FILE *filePtr, TimeStampStr *oldest, TimeStampStr *newest,
                UINT16 numStreams);
static INT32 GetTimeStamp (TimeStampStr *timeStamp, TimeStampAge age, UINT16 fileIndex);
static UINT16 CountStreams (void);
static INT32 IncludeStreamFiles (FILE *filePtr);
static INT32 CreateFileName (UINT16 fileIndex, char **fileName);
static BOOL VerifyFileIntegrity (const char *filename);
static BOOL TruncateFile (const char *fileName, UINT32 desiredFileSize);


/*****************************************************************************/
/**
 * @brief       Invoked from C# application to compile and build the viewable DAN file
 *
 *              This function compiles all of the #.stream files (stream files) that 
 *				were downloaded from the VCU and creates a viewable DAN file. It first 
 *              adds the XML configuration file followed by the RTDM header. It then
 *              adds all of the downloaded streams to the file, starting with the 
 *              oldest. 
 *              
 *              NOTE: the oldest and newest files are determined by timestamps
 *              in the file, not the # on the file name.
 * 
 *  @param managedFileName - updated with the viewable DAN file name (used by C# application)
 *  @param errorString - updated with an error message if an error was detected (used by C# application)
 * 
 *  @returns INT32 - 0 if all is well, non-zero otherwise
 *
 *//*
 * Revision History:
 *
 * Date & Author : 01SEP2016 - D.Smail
 * Description   : Original Release
 *
 *****************************************************************************/
__declspec(dllexport) INT32 __cdecl BuildViewableDanFile (char *managedFileName, char *errorString)
{

    UINT16 newestStreamFileIndex = INVALID_FILE_INDEX;	/* Stores the newest file index [0- (MAX_NUMBER_OF_STREAM_FILES - 1)] */
    UINT16 oldestStreamFileIndex = INVALID_FILE_INDEX; /* Stores the oldest file index [0- (MAX_NUMBER_OF_STREAM_FILES - 1)] */
    FILE *filePtr = NULL;	/* Holds the file pointer to the final DAN file that is created */
    TimeStampStr newestTimeStamp; /* Holds the newest time stamp from the newest file */
    TimeStampStr oldestTimeStamp; /* Holds the newest time stamp from the newest file */
    UINT16 streamCount = 0;	/* total number of streams in all #.stream files; required for RTDM header */ 
	char *fileName = NULL;	/* Filename for final DAN file */
	INT32 error = 0;	/* becomes non-zero if an error is detected during processing */
	char errorCode[10];

    /* Determine all current valid stream files */
    PopulateValidStreamFileList ();
    /* Get the oldest stream timestamp from every valid file */
    PopulateValidFileTimeStamps ();
    /* Sort the file indexes and timestamps from oldest to newest */
    SortValidFileTimeStamps ();

    /* Determine the newest file index */
    newestStreamFileIndex = GetNewestStreamFileIndex ();
    if (newestStreamFileIndex == INVALID_FILE_INDEX)
    {
		strcpy(errorString,"New Stream File Index Not Found");
		return (-1);
    }

    /* Get the newest timestamp (last one) in the newest file; used for RTDM header */
    error = GetTimeStamp (&newestTimeStamp, NEWEST_TIMESTAMP, newestStreamFileIndex);
	if (error != 0)
	{
		strcpy(errorString,"Couldn't process newest timestamp: Error Code = ");
		sprintf(errorCode, "%d", error);
		strcat(errorString, errorCode);
		return (-1);
	}

    /* Determine the oldest file index */
    oldestStreamFileIndex = GetOldestStreamFileIndex ();

    if (oldestStreamFileIndex == INVALID_FILE_INDEX)
    {
		strcpy(errorString,"Old Stream File Index Not Found");
		return (-1);
    }

    /* Get the oldest timestamp (first one) in the oldest file; used for RTDM header */
    error = GetTimeStamp (&oldestTimeStamp, OLDEST_TIMESTAMP, oldestStreamFileIndex);
	if (error != 0)
	{
		strcpy(errorString,"Couldn't process oldest timestamp: Error Code = ");
		sprintf(errorCode, "%d", error);
		strcat(errorString, errorCode);
		return (-1);
	}

    /* Scan through all valid files and count the number of streams in each file; used for
     * RTDM header */
    streamCount = CountStreams ();
	if (streamCount == 0)
	{
		strcpy(errorString,"No streams found in any of the #.stream files");
		return (-1);
	}

    /* Create filename and open it for writing */
    error = CreateViewableStreamFileName (&filePtr, &fileName);
	if (error != 0)
	{
		strcpy(errorString,"Couldn't create file name for viewable DAN file: Error Code = ");
		sprintf(errorCode, "%d", error);
		strcat(errorString, errorCode);
		return (-1);
	}

    if (filePtr == NULL)
    {
		strcpy(errorString,"Couldn't create viewable DAN FILE pointer");
		return (-1);
	}

    /* Include xml file */
    error = IncludeXMLFile (filePtr);
	if (error != 0)
	{
		strcpy(errorString,"Couldn't include XML configuration file in viewable DAN file: Error Code = ");
		sprintf(errorCode, "%d", error);
		strcat(errorString, errorCode);
		/* Close file pointer */
		os_io_fclose (filePtr);
		return (-1);
	}

    /* Include RTDM header */
    error = IncludeRTDMHeader (filePtr, &oldestTimeStamp, &newestTimeStamp, streamCount);
	if (error != 0)
	{
		strcpy(errorString,"Couldn't include RTDM header in viewable DAN file: Error Code = ");
		sprintf(errorCode, "%d", error);
		strcat(errorString, errorCode);
		/* Close file pointer */
		os_io_fclose (filePtr);
		return (-1);
	}

    /* Open each file .stream (oldest first) and concatenate */
    error = IncludeStreamFiles (filePtr);
	if (error != 0)
	{
		strcpy(errorString,"Couldn't include stream file(s) in viewable DAN file: Error Code = ");
		sprintf(errorCode, "%d", error);
		strcat(errorString, errorCode);
		/* Close file pointer */
		os_io_fclose (filePtr);
		return (-1);
	}

    /* Close file pointer */
    os_io_fclose (filePtr);
	strcpy(managedFileName, fileName);
	return 0;

}

/*****************************************************************************/
/**
 * @brief       Invoked from C# application to build a DAN file name
 *
 *              This function generates a DAN file name based on the car, consist
 *              and device Id
 * 
 *  @param managedFileName - updated with the viewable DAN file name (used by C# application)
 *  @param errorString - updated with an error message if an error was detected (used by C# application)
 * 
 *  @returns INT32 - 0 if all is well, non-zero otherwise
 *
 *//*
 * Revision History:
 *
 * Date & Author : 20DEC2016 - D.Smail
 * Description   : Original Release
 *
 *****************************************************************************/
__declspec(dllexport) INT32 __cdecl BuildDanFileName (char *managedFileName, char *errorString)
{
    FILE *filePtr = NULL;	/* Holds the file pointer to the final DAN file that is created */
	char *fileName = NULL;	/* Filename for final DAN file */
	INT32 error = 0;	/* becomes non-zero if an error is detected during processing */
	char errorCode[10];

    /* Create filename and open it for writing */
    error = CreateViewableStreamFileName (&filePtr, &fileName);
	if (error != 0)
	{
		strcpy(errorString,"Couldn't create file name for viewable DAN file: Error Code = ");
		sprintf(errorCode, "%d", error);
		strcat(errorString, errorCode);
		return (-1);
	}

    /* Close file pointer */
    os_io_fclose (filePtr);
	strcpy(managedFileName, fileName);
	return 0;

}

/*****************************************************************************/
/**
 * @brief       Populates the valid stream file (#.stream) list
 *
 *              This function determines if a stream file exists and is valid. The
 *              call to VerifyFileIntegrity() also attempts to fix a file if the
 *              last stream was not written in its entirety.
 *
 *  @returns INT32 - 0 if all is well, non-zero otherwise
 *
 *//*
 * Revision History:
 *
 * Date & Author : 01SEP2016 - D.Smail
 * Description   : Original Release
 *
 *****************************************************************************/
static INT32 PopulateValidStreamFileList (void)
{
    BOOL fileOK = FALSE; /* stream file is OK if TRUE */
    UINT16 fileIndex = 0; /* Used to index through all possible stream files */
    UINT16 arrayIndex = 0; /* increments every time a valid stream file is found */
	char *fileName = NULL; /* Holds the file name of the #.stream file */
	INT32 errorCode = 0; /* Non-zero indicates an error */

    /* Scan all files to determine what files are valid */
    for (fileIndex = 0; fileIndex < MAX_NUMBER_OF_STREAM_FILES ; fileIndex++)
    {
        /* Invalidate the index to ensure that */
        m_ValidStreamFileListIndexes[fileIndex] = INVALID_FILE_INDEX;
		errorCode = CreateFileName (fileIndex, &fileName);
		if (errorCode != 0)
		{
			return (-1);
		}
        fileOK = VerifyFileIntegrity (fileName);
        /* If valid file is found and is OK, populate the valid file array with the fileIndex */
        if (fileOK)
        {
            m_ValidStreamFileListIndexes[arrayIndex] = fileIndex;
            arrayIndex++;
        }
    }

	return (0);

}

/*****************************************************************************/
/**
 * @brief       Populates the time stamp list
 *
 *              This function scans all valid stream files and gets the
 *              oldest time stamp from each file.
 *
 *//*
 * Revision History:
 *
 * Date & Author : 01SEP2016 - D.Smail
 * Description   : Original Release
 *
 *****************************************************************************/
static void PopulateValidFileTimeStamps (void)
{
    UINT16 arrayIndex = 0; /* Used to index through time stamps and stream files */
    TimeStampStr timeStamp; /* temporary placeholder for the oldest time stamp in a stream file */

    /* Set all members to invalid indexes */
    for (arrayIndex = 0; arrayIndex < MAX_NUMBER_OF_STREAM_FILES ; arrayIndex++)
    {
        m_ValidTimeStampList[arrayIndex] = INVALID_TIME_STAMP;
    }

    /* Scan all the valid files. Get the oldest time stamp and populate the
     * time stamp with the seconds (epoch time).
     */
    arrayIndex = 0;
    while ((m_ValidStreamFileListIndexes[arrayIndex] != INVALID_FILE_INDEX)
                    && (arrayIndex < MAX_NUMBER_OF_STREAM_FILES ))
    {
        GetTimeStamp (&timeStamp, OLDEST_TIMESTAMP, m_ValidStreamFileListIndexes[arrayIndex]);
        m_ValidTimeStampList[arrayIndex] = timeStamp.seconds;
        arrayIndex++;
    }
}

/*****************************************************************************/
/**
 * @brief       Sorts the file indexes based on the stream time stamps
 *
 *              This function used the bubble sort algorithm to sort the
 *              file indexes based on the stream time stamps associated with
 *              each file. It assumes that the time stamps have been populated
 *              with the stream time stamps and that at least 1 second separates
 *              the stream time stamps. The sort orders oldest to newest and
 *              updates both the time stamps and the file indexes.
 *
 *
 *//*
 * Revision History:
 *
 * Date & Author : 01SEP2016 - D.Smail
 * Description   : Original Release
 *
 *****************************************************************************/
static void SortValidFileTimeStamps (void)
{
    UINT16 c = 0; /* Used in the bubble sort algorithm */
    UINT16 d = 0; /* Used in the bubble sort algorithm */
    UINT16 numValidTimestamps = 0; /* The number of valid time stamps (i.e stream files) */
    UINT32 swapTimestamp = 0; /* holding variable to place a time stamp */
    UINT16 swapIndex = 0; /* holding variable to place a file index */

    /* Find the number of valid timestamps */
    while ((m_ValidTimeStampList[numValidTimestamps] != INVALID_TIME_STAMP)
                    && (numValidTimestamps < MAX_NUMBER_OF_STREAM_FILES ))
    {
        numValidTimestamps++;
    }

    /* Bubble sort algorithm; sorts Stream file list and time stamp list */
    for (c = 0; c < (numValidTimestamps - 1); c++)
    {
        for (d = 0; d < numValidTimestamps - c - 1; d++)
        {
            /* For decreasing order use < */
            if (m_ValidTimeStampList[d] > m_ValidTimeStampList[d + 1])
            {
                swapTimestamp = m_ValidTimeStampList[d];
                m_ValidTimeStampList[d] = m_ValidTimeStampList[d + 1];
                m_ValidTimeStampList[d + 1] = swapTimestamp;

                swapIndex = m_ValidStreamFileListIndexes[d];
                m_ValidStreamFileListIndexes[d] = m_ValidStreamFileListIndexes[d + 1];
                m_ValidStreamFileListIndexes[d + 1] = swapIndex;
            }
        }
    }
}

/*****************************************************************************/
/**
 * @brief       Returns the newest file index (based on Stream time stamp)
 *
 *              This function assumes that the file indexes have been sorted
 *              prior to its call.
 *
 *  @returns UINT16 - newest file index
 *
 *//*
 * Revision History:
 *
 * Date & Author : 01SEP2016 - D.Smail
 * Description   : Original Release
 *
 *****************************************************************************/
static UINT16 GetNewestStreamFileIndex (void)
{
    UINT16 streamIndex = 0; /* Used to index through the valid stream file list */ 

    if (m_ValidStreamFileListIndexes[0] == INVALID_FILE_INDEX)
    {
        return INVALID_FILE_INDEX;
    }

    while ((m_ValidStreamFileListIndexes[streamIndex] != INVALID_FILE_INDEX)
                    && (streamIndex < MAX_NUMBER_OF_STREAM_FILES ))
    {
        streamIndex++;
    }

    /* Since the while loop has incremented one past the newest, subtract 1*/
    return (m_ValidStreamFileListIndexes[streamIndex - 1]);

}

/*****************************************************************************/
/**
 * @brief       Returns the oldest file index (based on Stream time stamp)
 *
 *              This function assumes that the file indexes have been sorted
 *              prior to its call.
 *
 *  @returns UINT16 - oldest file index
 *
 *//*
 * Revision History:
 *
 * Date & Author : 01SEP2016 - D.Smail
 * Description   : Original Release
 *
 *****************************************************************************/
static UINT16 GetOldestStreamFileIndex (void)
{
    return (m_ValidStreamFileListIndexes[0]);
}

/*****************************************************************************/
/**
 * @brief       Creates the viewable DAN file name and opens the file pointer to the file
 *
 *              Creates the filename based on the requirements from the ICD. This
 *              includes the consist, car, and device id along with the date/time.
 *
 *  @param filePtr - pointer to FILE pointer. Pointer to pointer
 *                   required so that the file pointer can be passed around to
 *                   other functions
 *  @param viewableStreamFileName - pointer to char pointer. Pointer to pointer
 *                   required so that the name can be passed back to the calling function
 *
 *  @returns INT32 - 0 if all is well, non-zero otherwise
 *
 *//*
 * Revision History:
 *
 * Date & Author : 01SEP2016 - D.Smail
 * Description   : Original Release
 *
 *****************************************************************************/
static INT32 CreateViewableStreamFileName (FILE **filePtr, char **viewableStreamFileName)
{
    char consistId[17]; /* Stores the consist id */
    char carId[17]; /* Stores the car id */
    char deviceId[17]; /* Stores the device id */
    char fConsistId[17]; /* Stores the consist id (from the file)*/
    char fCarId[17]; /* Stores the car id (from the file) */
    char fDeviceId[17]; /* Stores the device id (from the file) */
	UINT32 fConfigId = 0; /* Unused: need to scan it though to advance file pointer */
	UINT32 fConfigVersion = 0; /* Unused: need to scan it though to advance file pointer */
	FILE *pFile = NULL; /* File pointer to open up the car con dev file */
	const char *fileName = DRIVE_NAME DIRECTORY_NAME "CarConDev.dat";
    const char *extension = ".dan"; /* Required file extension */
    const char *rtdmFill = "rtdm____________"; /* Required filler */
    static char s_FileName[256]; /* Viewable DAN file name returned from function */
    char dateTime[64]; /* stores the date/time in the required format */
	INT32 errorCode = 0; /* Used  to determine if OS call was in error */
	char *errPtr = NULL;	/* used to determine if fgets() was successful */
	char value[16];         /* temporary storage for integer read from file */

    /* Set all default chars */
    memset (s_FileName, 0, sizeof(s_FileName));
    memset (consistId, '_', sizeof(consistId));
    memset (carId, '_', sizeof(carId));
    memset (deviceId, '_', sizeof(deviceId));
    
	memset (fConsistId, 0, sizeof(fConsistId));
    memset (fCarId, 0, sizeof(fCarId));
    memset (fDeviceId, 0, sizeof(fDeviceId));

    /* Terminate all strings with NULL */
    consistId[sizeof(consistId) - 1] = 0;
    carId[sizeof(carId) - 1] = 0;
    deviceId[sizeof(deviceId) - 1] = 0;


	/* Open the data configuration file and write the contents, need to extract contents to
	   create file name */
	if (os_io_fopen (fileName, "r+b", &pFile) == ERROR)
    {
        *filePtr = NULL;
		return (-1);
    }

	/* Read all parameters from the "CarConDev.dat"; must be in this order */
	errPtr = fgets(&value[0], sizeof(value), pFile);
	if (errPtr == NULL)
	{
		os_io_fclose(pFile);
		return (-2);
	}
	/* Convert the string to a number for configId */
	fConfigId = strtol (&value[0], (char **)NULL, 10);

	errPtr = fgets(&value[0], sizeof(value), pFile);
	if (errPtr == NULL)
	{
		os_io_fclose(pFile);
		return (-3);
	}
	/* Convert the string to a number for configVersion */
	fConfigVersion = strtol (&value[0], (char **)NULL, 10);

	errPtr = fgets(&fCarId[0], sizeof(fCarId), pFile);
	if (errPtr == NULL)
	{
		os_io_fclose(pFile);
		return (-4);
	}

	errPtr = fgets(&fConsistId[0], sizeof(fConsistId), pFile);
	if (errPtr == NULL)
	{
		os_io_fclose(pFile);
		return (-5);
	}

	errPtr = fgets(&fDeviceId[0], sizeof(fDeviceId), pFile);
	if (errPtr == NULL)
	{
		os_io_fclose(pFile);
		return (-6);
	}

	os_io_fclose(pFile);

	/* fgets will leave CR/LF in string; strtok() below removes them */
	strtok(&fCarId[0], "\n");
	strtok(&fCarId[0], "\r");
	strtok(&fConsistId[0], "\n");
	strtok(&fConsistId[0], "\r");
	strtok(&fDeviceId[0], "\n");
	strtok(&fDeviceId[0], "\r");

	/* Copy only the characters, memcpy used so that \0 isn't copied */
	memcpy(&consistId[0], &fConsistId[0], strlen(fConsistId));
	memcpy(&carId[0], &fCarId[0], strlen(fCarId));
	memcpy(&deviceId[0], &fDeviceId[0], strlen(fDeviceId));

    GetTimeDate (dateTime, sizeof(dateTime));

    /* Create the filename by concatenating in the proper order */
    strcat (s_FileName, DRIVE_NAME);
    strcat (s_FileName, DIRECTORY_NAME);
    strcat (s_FileName, consistId);
    strcat (s_FileName, carId);
    strcat (s_FileName, deviceId);
    strcat (s_FileName, rtdmFill);
    strcat (s_FileName, dateTime);
    strcat (s_FileName, extension);

    /* Try opening the file for writing and leave open */
    if (os_io_fopen (s_FileName, "wb+", filePtr) == ERROR)
    {
        *filePtr = NULL;
		return (-7);
    }

	/* Update the filename pointer */
	*viewableStreamFileName = &s_FileName[0];

    /* Return the file name */
    return (0);

}

/*****************************************************************************/
/**
 * @brief       Updates the file with XML configuration file
 *
 *              Gets a pointer from memory where the XML configuration file is stored and
 *              writes the contents to the file
 *
 *  @param filePtr - FILE pointer to the viewable file
 *
 *//*
 * Revision History:
 *
 * Date & Author : 01SEP2016 - D.Smail
 * Description   : Original Release
 *
 *****************************************************************************/
static INT32 IncludeXMLFile (FILE *filePtr)
{
	char buffer[FILE_READ_BUFFER_SIZE];	/* Stores bytes from the XML file */
	FILE *pFile = NULL;		/* File pointer to the XML file */
	const char *fileName = DRIVE_NAME DIRECTORY_NAME "RTDMConfiguration_PCU.xml";	/* Name of the XML file */
	UINT32 amount = 0;	/* amount of bytes read from the file */
	UINT32 errorCode = 0;	/* Used to detect errors */

	memset (buffer, 0, FILE_READ_BUFFER_SIZE);

	/* Open the XML configuration file and write the contents */
	if (os_io_fopen (fileName, "r+b", &pFile) == ERROR)
    {
        return (-1);
    }

    /* Assume file is opened and go the beginning of the file */
    errorCode = fseek (filePtr, 0L, SEEK_END);
	if (errorCode != 0)
	{
		return (-2);
	} 

	/* All's well, read from XML file and write to file */
    while (1)
    {
        /* Keep reading the stream file until all of the file is read */
        amount = fread (&buffer[0], 1, sizeof(buffer), pFile);

        /* End of file reached */
        if (amount == 0)
        {
            os_io_fclose (pFile);
            break;
        }

        /* Keep writing the stream file to the file */
        amount = fwrite (&buffer[0], amount, 1, filePtr);
        if (amount != 1)
        {
            return (-3);
        }
    }

	return 0;

}

/*****************************************************************************/
/**
 * @brief       Updates the viewable DAN file with the required RTDM header
 *
 *              Creates the required RTDM header and writes it to the file.
 *
 *  @param filePtr - FILE pointer to the file to be updated
 *  @param oldest - oldest stream time stamp that will be written to the file
 *  @param newest - newest stream time stamp that will be written to the file
 *  @param numStreams - the number of streams to be written in the file
 *
 *//*
 * Revision History:
 *
 * Date & Author : 01SEP2016 - D.Smail
 * Description   : Original Release
 *
 *****************************************************************************/
static INT32 IncludeRTDMHeader (FILE *filePtr, TimeStampStr *oldest, TimeStampStr *newest,
                UINT16 numStreams)
{
    RtdmHeaderStr rtdmHeader; /* RTDM header that will be populated and written to the file */
    char *delimiter = "RTDM"; /* RTDM delimiter in header */
    UINT32 rtdmHeaderCrc = 0; /* CRC calculation result on the postamble part of the header */
	char buffer[FILE_READ_BUFFER_SIZE];		/* holds bytes read from the file */
	FILE *pFile = NULL;		/* File pointer to the car con dev data */
	const char *fileName = DRIVE_NAME DIRECTORY_NAME "CarConDev.dat";	/* fully qualified file name */
	char carId[16];			/* holds the car id read from file */
	char consistId[16];		/* holds the consist id read from file */
	char deviceId[16];		/* holds the device id read from file */
	UINT32 configId = 0;	/* holds the configuration id read from file */
	UINT32 configVersion = 0; /* holds the version read from file */
	char *errPtr = NULL;	/* used to determine if fgets() was successful */
	char value[16];         /* temporary storage for integer read from file */
	INT32 errorCode = 0;	/* Used to determine if fwrite() failed */

	memset (buffer, 0, FILE_READ_BUFFER_SIZE);

	/* Open the Car, Consist, Device & parameter; used to populate the RTDM header */
	if (os_io_fopen (fileName, "r+b", &pFile) == ERROR)
    {
        return (-1);
    }

	/* Read all parameters from the "CarConDev.dat"; must be in this order */
	errPtr = fgets(&value[0], sizeof(value), pFile);
	if (errPtr == NULL)
	{
		os_io_fclose(pFile);
		return (-2);
	}
	/* Convert the string to a number for configId */
	configId = strtol (&value[0], (char **)NULL, 10);

	errPtr = fgets(&value[0], sizeof(value), pFile);
	if (errPtr == NULL)
	{
		os_io_fclose(pFile);
		return (-3);
	}
	/* Convert the string to a number for configVersion */
	configVersion = strtol (&value[0], (char **)NULL, 10);

	errPtr = fgets(&carId[0], sizeof(carId), pFile);
	if (errPtr == NULL)
	{
		os_io_fclose(pFile);
		return (-4);
	}

	errPtr = fgets(&consistId[0], sizeof(consistId), pFile);
	if (errPtr == NULL)
	{
		os_io_fclose(pFile);
		return (-5);
	}

	errPtr = fgets(&deviceId[0], sizeof(deviceId), pFile);
	if (errPtr == NULL)
	{
		os_io_fclose(pFile);
		return (-6);
	}

	/* Close file since we don't need it anymore */
	os_io_fclose(pFile);

	/* fgets will leave CR/LF in string; strtok() below removes them */
	strtok(&carId[0], "\n");
	strtok(&carId[0], "\r");
	strtok(&consistId[0], "\n");
	strtok(&consistId[0], "\r");
	strtok(&deviceId[0], "\n");
	strtok(&deviceId[0], "\r");


	memset (&rtdmHeader, 0, sizeof(rtdmHeader));

    if (strlen (delimiter) > sizeof(rtdmHeader.preamble.delimiter))
    {
        return (-7);
    }
    else
    {
        memcpy (&rtdmHeader.preamble.delimiter[0], delimiter, strlen (delimiter));
    }

    /* Endianness - Always BIG */
    rtdmHeader.preamble.endianness = BIG_ENDIAN;
    /* Header size */
    rtdmHeader.preamble.headerSize = htons (sizeof(rtdmHeader));
    /* Header Version - Always 2 */
    rtdmHeader.postamble.headerVersion = RTDM_HEADER_VERSION;

	strcpy (&rtdmHeader.postamble.consistId[0], &consistId[0]);
    strcpy (&rtdmHeader.postamble.carId[0], &carId[0]);
    strcpy (&rtdmHeader.postamble.deviceId[0], &deviceId[0]);

    /* Data Recorder ID - from .xml file */
    rtdmHeader.postamble.dataRecordId = htons (configId);

    /* Data Recorder Version - from .xml file */
    rtdmHeader.postamble.dataRecordVersion = htons (configVersion);

    /* First TimeStamp -  seconds */
    rtdmHeader.postamble.firstTimeStampSecs = htonl (oldest->seconds);

    /* First TimeStamp - msecs */
    rtdmHeader.postamble.firstTimeStampMsecs = htons (oldest->msecs);

    /* Last TimeStamp -  seconds */
    rtdmHeader.postamble.lastTimeStampSecs = htonl (newest->seconds);

    /* Last TimeStamp - msecs */
    rtdmHeader.postamble.lastTimeStampMsecs = htons (newest->msecs);

    rtdmHeader.postamble.numStreams = htonl (numStreams);

    /* The CRC is calculated on the "postamble" part of the RTDM header only */
    rtdmHeaderCrc = 0;
    rtdmHeaderCrc = crc32 (rtdmHeaderCrc, ((UINT8 *) &rtdmHeader.postamble),
                    sizeof(rtdmHeader.postamble));
    rtdmHeader.preamble.headerChecksum = htonl (rtdmHeaderCrc);

    /* Update the file with the RTDM header */
    errorCode = fwrite (&rtdmHeader, sizeof(rtdmHeader), 1, filePtr);
	if (errorCode != 1)
	{
		return (-8);
	}

	return (0);

}

/*****************************************************************************/
/**
 * @brief       Gets a time stamp from a stream file (#.stream)
 *
 *              Opens the desired stream file and either gets the newest or oldest
 *              time stamp in the file.
 *
 *  @param timeStamp - populated with either the oldest or newest time stamp
 *  @param age - informs function to get either the oldest or newest time stamp
 *  @param fileIndex - indicates the name of the stream file (i.e 0 cause "0.stream" to be opened)
 *
 *//*
 * Revision History:
 *
 * Date & Author : 01SEP2016 - D.Smail
 * Description   : Original Release
 *
 *****************************************************************************/
static INT32 GetTimeStamp (TimeStampStr *timeStamp, TimeStampAge age, UINT16 fileIndex)
{
    FILE *pFile = NULL;
    UINT16 sIndex = 0;
    UINT8 buffer[1];
    size_t amountRead = 0;
    StreamHeaderContent streamHeaderContent;
    char *fileName = NULL;
	INT32 errorCode = 0;

    /* Reset the stream header. If no valid streams are found, then the time stamp structure will
     * have "0" in it. */
    memset (&streamHeaderContent, 0, sizeof(streamHeaderContent));

    errorCode = CreateFileName (fileIndex, &fileName);
	if (errorCode != 0)
	{
		return (-1);
	}

    if (os_io_fopen (fileName, "r+b", &pFile) == ERROR)
    {
        return (-2);
    }

    while (1)
    {
        /* TODO revisit and read more than 1 byte at a time
         * Search for delimiter. Design decision to read only a byte at a time. If more than 1 byte is
         * read into a buffer, than more complex logic is needed */
        amountRead = fread (&buffer, sizeof(UINT8), sizeof(buffer), pFile);

        /* End of file reached */
        if (amountRead != sizeof(buffer))
        {
            break;
        }

        if (buffer[0] == m_StreamHeaderDelimiter[sIndex])
        {
            sIndex++;
            /* Delimiter found if inside of "if" is reached */
            if (sIndex == strlen (m_StreamHeaderDelimiter))
            {
                /* Read the entire stream header, which occurs directly after the stream, delimiter */
                fread (&streamHeaderContent, sizeof(streamHeaderContent), 1, pFile);

                /* Get out of while loop on first occurrence */
                if (age == OLDEST_TIMESTAMP)
                {
                    break;
                }
                else
                {
                    sIndex = 0;
                }
            }
        }
        else
        {
            sIndex = 0;
        }
    }

    /* Copy the stream header time stamp into the passed argument */
    timeStamp->seconds = ntohl (streamHeaderContent.postamble.timeStampUtcSecs);
    timeStamp->msecs = ntohs (streamHeaderContent.postamble.timeStampUtcMsecs);

    os_io_fclose (pFile);

	return 0;

}

/*****************************************************************************/
/**
 * @brief       Counts the number of  valid streams in the stream (#.stream) files
 *
 *              This file scans through the list of all valid stream files
 *              counts the total number of streams in all of the valid stream files.
 *
 *  @returns UINT16 - the total number of streams encountered in the valid stream
 *                    files; 0 indicates an error was found or no streams detected
 *
 *//*
 * Revision History:
 *
 * Date & Author : 01SEP2016 - D.Smail
 * Description   : Original Release
 *
 *****************************************************************************/
static UINT16 CountStreams (void)
{
    UINT16 streamCount = 0; /* maintains the total the number of streams */
    UINT16 fileIndex = 0; /* used to scan through all valid stream files */
    FILE *streamFilePtr = NULL; /* File pointer to read stream files (#.stream) */
    UINT8 buffer[1]; /* file read buffer */
    UINT32 amountRead = 0; /* amount of data read from stream file */
    UINT16 sIndex = 0; /* indexes into stream delimiter */
    char *fileName = NULL;	/* holds the filename of the #.stream file */
	INT32 errorCode = 0;	/* Becomes non-zero if an error is detected */

    /* Scan through all valid stream files and tally the number of occurrences of "STRM" */
    while ((m_ValidStreamFileListIndexes[fileIndex] != INVALID_FILE_INDEX)
                    && (fileIndex < MAX_NUMBER_OF_STREAM_FILES ))
    {
		errorCode = CreateFileName (m_ValidStreamFileListIndexes[fileIndex], &fileName);
		if (errorCode != 0)
		{
			return (0);
		}
        /* Open the stream file for reading */
        if (os_io_fopen (fileName, "r+b", &streamFilePtr) == ERROR)
        {
            return (0);
        }
        while (1)
        {
            /* Search for delimiter */
            amountRead = fread (&buffer[0], sizeof(UINT8), sizeof(buffer), streamFilePtr);

            /* End of file reached */
            if (amountRead != sizeof(buffer))
            {
                break;
            }

            if (buffer[0] == m_StreamHeaderDelimiter[sIndex])
            {
                sIndex++;
                if (sIndex == strlen (m_StreamHeaderDelimiter))
                {
                    /* Stream delimiter found; increment stream count */
                    streamCount++;
                    sIndex = 0;
                }
            }
			else if ((buffer[0] == m_StreamHeaderDelimiter[0]) && (sIndex == 1))
            {
				/* This handles the case where at least 1 binary 'S' leads the "STRM" */
                sIndex = 1;
            }
			else
			{
                sIndex = 0;
			}
        }

        sIndex = 0;

        /* close stream file */
        os_io_fclose (streamFilePtr);

        fileIndex++;

    }

    return (streamCount);

}

/*****************************************************************************/
/**
 * @brief       Updates the file with all valid stream (*.stream) files
 *
 *              This file scans through the list of all valid stream files
 *              and appends each stream file to the file.
 *
 *  @param filePtr - file pointer to the soon to be created viewable DAN file (writes occur)
 *
 *  @return INT32 - 0 if all is well, non-zero otherwise
 *//*
 * Revision History:
 *
 * Date & Author : 01SEP2016 - D.Smail
 * Description   : Original Release
 *
 *****************************************************************************/
static INT32 IncludeStreamFiles (FILE *filePtr)
{
    UINT16 fileIndex = 0; /* Used to scan through m_ValidStreamFileListIndexes */
    FILE *streamFilePtr = NULL; /* Used to open the stream file for reading */
    UINT8 buffer[FILE_READ_BUFFER_SIZE]; /* Stores the data read from the stream file */
    UINT32 amount = 0; /* bytes or blocks read or written */
    char *fileName = NULL;	/* holds the #.stream file name */
	INT32 errorCode = 0;	/* Becomes non-zero if an error is detected */

    /* Scan through all valid stream files. This list is ordered oldest to newest. */
    while ((m_ValidStreamFileListIndexes[fileIndex] != INVALID_FILE_INDEX)
                    && (fileIndex < MAX_NUMBER_OF_STREAM_FILES ))
    {
        errorCode = CreateFileName (m_ValidStreamFileListIndexes[fileIndex], &fileName);
		if (errorCode != 0)
		{
			return (-1);
		}
        /* Open the stream file for reading */
        if (os_io_fopen (fileName, "r+b", &streamFilePtr) == ERROR)
        {
            return (-2);
        }
        else
        {
            /* All's well, read from stream file and write to the file */
            while (1)
            {
                /* Keep reading the stream file until all of the file is read */
                amount = fread (&buffer[0], 1, sizeof(buffer), streamFilePtr);

                /* End of file reached */
                if (amount == 0)
                {
                    os_io_fclose (streamFilePtr);
                    break;
                }

                /* Keep writing the stream file to the file */
                amount = fwrite (&buffer[0], amount, 1, filePtr);
                if (amount != 1)
                {
                    return (-3);
                }
            }
        }

        fileIndex++;

    }

	return (0);
}

/*****************************************************************************/
/**
 * @brief       Creates a filename #.stream file
 *
 *              This function creates a #.stream file based on the drive/directory
 *              and file index
 *
 *  @param fileName - path/filename of file to be verified
 *  @param char ** - address of pointer to the newly created file name
 *
 *  @return INT32 - 0 if all is well, non-zero otherwise
 *//*
 * Revision History:
 *
 * Date & Author : 01SEP2016 - D.Smail
 * Description   : Original Release
 *
 *****************************************************************************/
static INT32 CreateFileName (UINT16 fileIndex, char **fileName)
{
    static char s_FileName[100]; /* Stores the newly created filename */
    const char *extension = ".stream"; /* Holds the extension for the file */
	UINT16 convertCount = 0; /* return value from sprintf; should be equal to the number of format specifiers */

    memset (s_FileName, 0, sizeof(s_FileName));

    strcat (s_FileName, DRIVE_NAME);
    strcat (s_FileName, DIRECTORY_NAME);

    /* Append the file index to the drive and directory */
    convertCount = sprintf (&s_FileName[strlen (s_FileName)], "%u", fileIndex);
    if (convertCount != 1)
	{
		return (-1);
	}

	/* Append the extension */
    strcat (s_FileName, extension);
	*fileName = s_FileName;

    return (0);
}

/*****************************************************************************/
/**
 * @brief       Verifies a #.stream file integrity
 *
 *              This function verifies that the final stream "STRM" section
 *              in the file has the correct number of bytes. IMPORTANT: There
 *              is no CRC check performed. It assumes that all bytes prior to the last
 *              stream are written correctly. This check is performed in case a
 *              file write was interrupted because of a power cycle or the like.
 *
 *  @param fileName - path/filename of file to be verified
 *
 *  @return BOOL - TRUE if the file is valid or made valid; FALSE if the file doesn't exist
 *//*
 * Revision History:
 *
 * Date & Author : 01SEP2016 - D.Smail
 * Description   : Original Release
 *
 *****************************************************************************/
static BOOL VerifyFileIntegrity (const char *filename)
{
    FILE *pFile = NULL; /* File pointer */
    UINT8 buffer; /* Stores byte read file */
    UINT32 amountRead = 0; /* Amount of bytes read on a single read */
    UINT32 expectedBytesRemaining = 0; /* Amount of bytes that should be remaining in the file */
    UINT32 byteCount = 0; /* Total amount of bytes read */
    INT32 lastStrmIndex = -1; /* Byte offset to the last "STRM" in the file, Intentionally set to -1 for a n error check */
    UINT16 sIndex = 0; /* Used to index in to streamHeaderDelimiter */
    StreamHeaderStr streamHeader; /* Overlaid on bytes read from the file */
    BOOL purgeResult = FALSE; /* Becomes TRUE if file truncated successfully */
    UINT16 sampleSize = 0;	/* Used to hold the sample size after accounting for network byte ordering */
	INT32 errorCode = 0; /* Used to detect errors in function calls */

    /* File doesn't exist */
    if (os_io_fopen (filename, "r+b", &pFile) == ERROR)
    {
        return FALSE;
    }

    /* Keep searching the entire file for "STRM". When the end of file is reached
     * "lastStrmIndex" will hold the byte offset in "filename" to the last "STRM"
     * encountered.
     */
    while (1)
    {
        amountRead = fread (&buffer, sizeof(UINT8), 1, pFile);
        byteCount += amountRead;

        /* End of file reached */
        if (amountRead != 1)
        {
            break;
        }

        /* Search for delimiter */
        if (buffer == m_StreamHeaderDelimiter[sIndex])
        {
            sIndex++;
            if (sIndex == strlen (m_StreamHeaderDelimiter))
            {
                /* Set the index strlen(streamHeaderDelimiter) backwards so that it points at the
                 * first char in streamHeaderDelimiter */
                lastStrmIndex = (INT32) (byteCount - strlen (m_StreamHeaderDelimiter));
                sIndex = 0;
            }
        }
        else
        {
            sIndex = 0;
        }
    }

    /* Since lastStrmIndex was never updated in the above loop (maintained its function
     * invocation value), then no "STRM"s were discovered.
     */
    if (lastStrmIndex == -1)
    {
        os_io_fclose (pFile);
        return (FALSE);
    }

    /* Move the file pointer to the last stream */
    errorCode = fseek (pFile, (INT32) lastStrmIndex, SEEK_SET);
	if (errorCode != 0)
	{
		return (FALSE);
	}

    /* Clear the stream header and overlay it on to the last stream header */
    memset (&streamHeader, 0, sizeof(streamHeader));

    amountRead = fread (&streamHeader, 1, sizeof(streamHeader), pFile);
    expectedBytesRemaining = byteCount - ((UINT32) lastStrmIndex + sizeof(streamHeader) - 8);

    /* Verify the entire streamHeader amount could be read and the expected bytes remaining are in fact there */
    sampleSize = ntohs (streamHeader.content.postamble.sampleSize);
    if ((sampleSize != expectedBytesRemaining) || (amountRead != sizeof(streamHeader)))
    {
        os_io_fclose (pFile);

        /* If lastStrmIndex = 0, that indicates the first and only stream in the file is corrupted and therefore the
         * entire file should be deleted. */
        if (lastStrmIndex == 0)
        {
            remove (filename);
            return (FALSE);
        }

        /* Remove the last "STRM" from the end of the file */
        purgeResult = TruncateFile (filename, (UINT32) lastStrmIndex);
        return (purgeResult);

    }

    os_io_fclose (pFile);
    return (TRUE);
}


/*****************************************************************************/
/**
 * @brief       Removes all desired data from the end of a file
 *
 *              This function creates a temporary file and copies the first
 *              "desiredFileSize" bytes from "fileName" into the temporary
 *              file. It then deletes the original fileName from the file system.
 *              It then renames the temporary filename to "fileName".
 *
 *  @param fileName - path/filename of file to delete some end of file data
 *  @param desiredFileSize - the number of bytes to maintain from the original file
 *
 *  @return BOOL - TRUE if all OS/file calls; FALSE otherwise
 *//*
 * Revision History:
 *
 * Date & Author : 01SEP2016 - D.Smail
 * Description   : Original Release
 *
 *****************************************************************************/
static BOOL TruncateFile (const char *fileName, UINT32 desiredFileSize)
{
    UINT8 buffer[FILE_READ_BUFFER_SIZE];	/* Stores bytes read from stream file */
    UINT32 amountRead = 0;		/* amount of bytes read from file */
    FILE *pReadFile = NULL;		/* FILE pointer to corrupt stream file */
    FILE *pWriteFile = NULL;	/* FILE pointer to new fixed stream file */
    UINT32 byteCount = 0;		/* maintains the number of bytes read */
    UINT32 remainingBytesToWrite = 0;	/* maintains the remaining number of bytes to write */
    INT32 osCallReturn = 0;		/* Stores OS file function call status; used to detect errors */
    const char *tempFileName = DRIVE_NAME DIRECTORY_NAME "temp.stream";	/* temporary fix file */

    /* Open the file to be truncated for reading */
    if (os_io_fopen (fileName, "rb", &pReadFile) == ERROR)
    {
        return (FALSE);
    }

    /* Open the temporary file where the first "desiredFileSize" bytes will be written */
    if (os_io_fopen (tempFileName, "wb+", &pWriteFile) == ERROR)
    {
        os_io_fclose (pReadFile);
        return (FALSE);
    }

    /* Ensure the respective file pointers are set to the beginning of the file */
    osCallReturn = fseek (pWriteFile, 0L, SEEK_SET);
    if (osCallReturn != 0)
    {
        os_io_fclose (pWriteFile);
        os_io_fclose (pReadFile);
        return (FALSE);
    }
    osCallReturn = fseek (pReadFile, 0L, SEEK_SET);
    if (osCallReturn != 0)
    {
        os_io_fclose (pWriteFile);
        os_io_fclose (pReadFile);
        return (FALSE);
    }

    while (1)
    {
        /* Read the file */
        amountRead = fread (&buffer, 1, sizeof(buffer), pReadFile);
        byteCount += amountRead;

        /* End of file reached, should never enter here because file updates should occur and exit before the end of file
         * is reached.  */
        if (amountRead == 0)
        {
            os_io_fclose (pWriteFile);
            os_io_fclose (pReadFile);
            return (FALSE);
        }

        /* Check if the amount of bytes read is less than the desired file size */
        if (byteCount < desiredFileSize)
        {
            osCallReturn = (INT32) fwrite (buffer, sizeof(UINT8), sizeof(buffer), pWriteFile);
            if (osCallReturn != (INT32) sizeof(buffer))
            {
                os_io_fclose (pWriteFile);
                os_io_fclose (pReadFile);
                return (FALSE);
            }
        }
        else
        {
            /* Calculate how many bytes to write to the file now that the system has read more
             * bytes than the desired amount to write.
             */
            remainingBytesToWrite = sizeof(buffer) - (byteCount - desiredFileSize);

            osCallReturn = (INT32) fwrite (buffer, 1, remainingBytesToWrite, pWriteFile);
            if (osCallReturn != (INT32) remainingBytesToWrite)
            {
                os_io_fclose (pWriteFile);
                os_io_fclose (pReadFile);
                return (FALSE);
            }

            os_io_fclose (pWriteFile);
            os_io_fclose (pReadFile);

            /* Delete the file that was being truncated */
            osCallReturn = remove (fileName);
            if (osCallReturn != 0)
            {
                return (FALSE);
            }
            /* Rename the temporary file to the fileName */
            rename (tempFileName, fileName);
            if (osCallReturn != 0)
            {
                return (FALSE);
            }

            return (TRUE);
        }
    }

}

