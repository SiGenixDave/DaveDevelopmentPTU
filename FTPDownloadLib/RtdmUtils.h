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
 * Project    : Communication Controller (Embedded)
 *//**
 * @file RtdmUtils.h
 *//*
 *
 * Revision : 01SEP2016 - D.Smail : Original Release
 *
 *****************************************************************************/

#ifndef RTDMUTILS_H_
#define RTDMUTILS_H_

/*******************************************************************
 *
 *     C  O  N  S  T  A  N  T  S
 *
 *******************************************************************/

/* RTDM Header Version */
#define RTDM_HEADER_VERSION         2

/* Stream Header Verion */
#define STREAM_HEADER_VERSION       2

/* Must be 0 as defined in the ICD; placed in RTDM and STRM header */
#define BIG_ENDIAN                 0


/*******************************************************************
 *
 *     E  N  U  M  S
 *
 *******************************************************************/

/*******************************************************************
 *
 *    S  T  R  U  C  T  S
 *
 *******************************************************************/
/* Main Stream buffer to be send out via MD message */
typedef struct
{
    UINT32 seconds;
    UINT16 msecs;
    UINT8 accuracy;
} TimeStampStr;

/* Structure to contain header for stream data */
typedef struct
{
    TimeStampStr timeStamp;
    UINT16 count;
} DataSampleStr;

typedef struct
{
    UINT8 endianness;
    UINT16 headerSize;
    UINT32 headerChecksum;
} StreamHeaderPreambleStr;

/* Structure created to support ease of CRC calculation for preamble's header checksum */
typedef struct
{
    UINT8 version;
    UINT8 consistId[16];
    UINT8 carId[16];
    UINT8 deviceId[16];
    UINT16 dataRecorderId;
    UINT16 dataRecorderVersion;
    UINT32 timeStampUtcSecs;
    UINT16 timeStampUtcMsecs;
    UINT8 timeStampUtcAccuracy;
    UINT32 mdsStreamReceiveSecs;
    UINT16 mdsStreamReceiveMsecs;
    UINT16 sampleSize;
    UINT32 samplesChecksum;
    UINT16 numberOfSamples;
} StreamHeaderPostambleStr;

typedef struct
{
    StreamHeaderPreambleStr preamble;
    StreamHeaderPostambleStr postamble;
} StreamHeaderContent;

/* Structure to contain variables in the Stream header of the message */
typedef struct
{
    char Delimiter[4];
    StreamHeaderContent content;
} StreamHeaderStr;

typedef struct 
{
    UINT32 seconds;
    UINT32 nanoseconds;
} RTDMTimeStr;
/*******************************************************************
 *
 *    E  X  T  E  R  N      V  A  R  I  A  B  L  E  S
 *
 *******************************************************************/

/*******************************************************************
 *
 *    E  X  T  E  R  N      F  U  N  C  T  I  O  N  S
 *
 *******************************************************************/

#endif /* RTDMUTILS_H_ */
