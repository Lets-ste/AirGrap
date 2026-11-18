import ctypes

lib = ctypes.CDLL("./add.dll")

print(lib.summ(2,5))