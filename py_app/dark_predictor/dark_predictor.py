import os
import sys
import platform
import copy
import ctypes


class PredictResult(ctypes.Structure):
    _fields_ = [('class_id', ctypes.c_int), ('x', ctypes.c_float),
                ('y', ctypes.c_float), ('w', ctypes.c_float), ('h', ctypes.c_float), ('probability', ctypes.c_float)]


PREDICT_RESULT_CALLBACK = ctypes.CFUNCTYPE(ctypes.c_void_p, ctypes.POINTER(PredictResult), ctypes.c_int)


class DarkPredictor:
    """wrapper of DarkPredictor.dll/so"""

    def __init__(self):
        """load library"""
        dir = os.path.dirname(__file__)
        lib_file = "./bin/DarkPredictor.dll"
        if platform.system() != "Windows":
            lib_file = "./bin/DarkPredictor.so"

        lib_file = os.path.join(dir, lib_file)

        try:
            lib = ctypes.CDLL(lib_file)

            create_predictor = lib.create_predictor
            create_predictor.argtypes = None
            create_predictor.restype = ctypes.c_void_p

            destroy_predictor = lib.destroy_predictor
            destroy_predictor.argtypes = [ctypes.c_void_p]
            destroy_predictor.restype = None

            set_log = lib.set_log
            set_log.argtypes = [ctypes.c_void_p, ctypes.c_char_p]
            set_log.restype = None

            load = lib.load
            load.argtypes = [ctypes.c_void_p, ctypes.c_char_p, ctypes.c_char_p]
            load.restype = None

            predict_image = lib.predict_image
            predict_image.argtypes = [
                ctypes.c_void_p, ctypes.c_char_p, ctypes.c_int, ctypes.c_int, ctypes.c_int, PREDICT_RESULT_CALLBACK]
            predict_image.restype = ctypes.c_void_p

            predict_image_file = lib.predict_image_file
            predict_image_file.argtypes = [
                ctypes.c_void_p, ctypes.c_char_p, PREDICT_RESULT_CALLBACK]
            predict_image_file.restype = ctypes.c_void_p

            self.c_create_predictor = create_predictor
            self.c_destroy_predictor = destroy_predictor
            self.c_set_log = set_log
            self.c_load = load
            self.c_predict_image = predict_image
            self.c_predict_image_file = predict_image_file

            self.predictor = create_predictor()

        except Exception as e:
            print(e)
            raise

    def destroy(self):
        self.c_destroy_predictor(self.predictor)

    def set_log(self, log_file):
        lf = log_file.encode('utf-8')
        self.c_set_log(self.predictor, ctypes.cast(lf, ctypes.c_char_p))

    def load(self, cfg, weights):
        cs = cfg.encode('utf-8')
        wt = weights.encode('utf-8')
        self.c_load(self.predictor, ctypes.cast(
            cs, ctypes.c_char_p), ctypes.cast(wt, ctypes.c_char_p))

    def predict_file(self, image_path):
        img = image_path.encode('utf-8')
        rst = []
        cb = PREDICT_RESULT_CALLBACK(
            lambda p, n: [rst.append(copy.deepcopy(p[n])) for n in range(n)])
        self.c_predict_image_file(
            self.predictor, ctypes.cast(img, ctypes.c_char_p), cb)
        return rst

    def predict_image(self, data, width, height, channels):
        rst = []
        cb = PREDICT_RESULT_CALLBACK(
            lambda p, n: [rst.append(copy.deepcopy(p[n])) for n in range(n)])
        self.c_predict_image(self.predictor, data,
                           width, height, channels, cb)
        return rst
