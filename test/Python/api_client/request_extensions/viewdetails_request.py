#!/usr/bin/env python
# -*- coding: utf-8 -*-

from requests import Request
import json

from api_client.response_wrappers.viewdetails_response_wrapper import ViewDetailsResponseWrapper

class ViewDetailsRequest(Request):

	URL = "https://investor-api.herokuapp.com/api/1.0/users"

	def __init__(self, session, token):
		self.token = token
		self.make_request()
		self.session = session

	def make_request(self):
		headers = {
			"Content-Type": "application/json",
			"charset": "UTF-8",
			"Authorization": "Bearer " + str(self.token)
		}
		super().__init__('GET', url = self.URL, headers = headers)

	def get_response(self):
		response = self.session.send(super().prepare())
		wrapped_response = ViewDetailsResponseWrapper(response)
		response.close()
		return wrapped_response