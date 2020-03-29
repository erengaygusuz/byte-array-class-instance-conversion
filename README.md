# Byte Array and Class Instance Conversion

## Description
This is the example of conversion byte array and any class instance.
You can convert byte array to any class instance or any class instance to byte array.

## Usage
There is Conversion class which is static. You can use its functions. 
There are two important functions. ObjectToByteArray() and ByteArrayToObject()

### Example for conversion byte array to class instance

Give your byte array like a paramater to ByteArrayToObject() function

- Conversion.ByteArrayToObject(yourByteArray);

### Example for conversion class instance to byte array

Give your byte array and conversion order array like a paramater to ObjectToByteArray() function

- Conversion.ObjectToByteArray(yourClassInstance, conversionOrderArray);
