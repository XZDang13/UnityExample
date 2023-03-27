import json
import time
import messages
from server import Server

class Simulator:
    def __init__(self, host="127.0.0.1", port=5004) -> None:
        self.server = Server(host, port)
        
    def start_server(self):
        self.server.start_server()
        
    def update_target_object_transform(self, position, euler):
        position_dict = {}
        for dim, value in zip(["x", "y", "z"], position):
            position_dict[dim] = float(value)
        
        euler_dict = {}
        for dim, value in zip(["x", "y", "z"], euler):
            euler_dict[dim] = float(value)
            
        parameter_message = messages.TargetObjectTransformMessage(position_dict, euler_dict)
        message = messages.RequestMessage("update", parameter_message.to_json())
        self.server.send([message.to_json()])
    
    def generate_samples(self, num_sample):
        message = messages.RequestMessage("sample", str(num_sample))
        self.server.send([message.to_json()])
        
        response = messages.ResponseMessage.from_json(self.server.recive()[0])
        point_cloud_message = messages.PointCloudMessage.from_json(response.value)
        point_messages = []
        for point_json in point_cloud_message.point_json:
            point_message = messages.PointMessage.from_json(point_json)
            point_messages.append(point_message)
            
        return point_messages