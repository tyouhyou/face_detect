import dark_predictor.dark_predictor

pred = dark_predictor.dark_predictor.DarkPredictor()
pred.set_log("log.txt")
pred.load("E:\\workspace\\face_detect\\data\\facex.cfg",
          "E:\\workspace\\face_detect\\data\\facex.weights")
rst = pred.predict_file("E:\\test_images\\i.jpg")
if None != rst:
    for r in rst:
        print('class_id: {0}, prob: {1}, x:{2}, y:{3}, w:{4}, h: {5}'.format(
            r.class_id, r.probability, r.x, r.y, r.w, r.h))
pred.destroy()
